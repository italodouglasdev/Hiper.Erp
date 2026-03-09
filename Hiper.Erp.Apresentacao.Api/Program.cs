using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.FormasPagamentos;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Mensageria;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas;
using Hiper.Erp.Aplicacao.Mapeadores;
using Hiper.Erp.Aplicacao.Servicos.Agentes;
using Hiper.Erp.Aplicacao.Servicos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Servicos.Produtos;
using Hiper.Erp.Aplicacao.Servicos.ServicosExternos;
using Hiper.Erp.Aplicacao.Servicos.Vendas;
using Hiper.Erp.Apresentacao.Api.Middlewares;
using Hiper.Erp.Infraestrutura.Bancos;
using Hiper.Erp.Infraestrutura.Bancos.SGDBs.Fabricas;
using Hiper.Erp.Infraestrutura.Cache;
using Hiper.Erp.Infraestrutura.Mensageria;
using Hiper.Erp.Infraestrutura.Repositorios.Agentes;
using Hiper.Erp.Infraestrutura.Repositorios.FormasPagamentos;
using Hiper.Erp.Infraestrutura.Repositorios.Produtos;
using Hiper.Erp.Infraestrutura.Repositorios.ServicoExternos.Tenant;
using Hiper.Erp.Infraestrutura.Repositorios.Vendas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// 1. Configurações de Infraestrutura Base
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


// 2. Servidor de Administração (Centralizado)
builder.Services.AddScoped<IServicoAdministrador>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    httpClient.BaseAddress = new Uri(builder.Configuration["HiperAdm:Url"] ?? "https://localhost:7125");
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    return new ServicoHiperAdm(httpClient, httpContextAccessor);
});

// 3. Contexto do Tenant (Scoped para cada requisição)
builder.Services.AddScoped<ITenantContext, TenantContext>();

// 4. Configuração Dinâmica do DbContext (Multi-Tenancy)
builder.Services.AddScoped<RetaguardaDbContext>(provider =>
{
    var tenantContext = provider.GetRequiredService<ITenantContext>();

    return FabricaConexoes.CriarContexto(tenantContext.TipoSgdb, tenantContext.ConnectionString);

});


// 5. UnitOfWork e Repositórios (Agora recebem o DbContext injetado)
builder.Services.AddScoped<IRepositorioAgentes, RepositorioAgentesDb>();
builder.Services.AddScoped<IRepositorioProdutos, RepositorioProdutosDb>();
builder.Services.AddScoped<IRepositorioFormasPagamentos, RepositorioFormasPagamentosDb>();
builder.Services.AddScoped<IRepositorioVendas, RepositorioVendasDb>();
builder.Services.AddScoped<IRepositorioVendasItens, RepositorioVendasItensDb>();


// 6. Serviços de Aplicação
builder.Services.AddScoped<IServicoAgentes, ServicoAgentes>();
builder.Services.AddScoped<IServicoProdutos, ServicoProdutos>();
builder.Services.AddScoped<IServicoFormasPagamentos, ServicoFormasPagamentos>();
builder.Services.AddScoped<IServicoVendas, ServicoVendas>();
builder.Services.AddScoped<IServicoVendasItens, ServicoVendasItens>();


// 7. Serviço de Mensageria (RabbitMQ)
// Registrado como Singleton pois a conexão com o RabbitMQ pode ser reutilizada entre requisições.
// A implementação cria conexões sob demanda a cada publicação, então Scoped também funcionaria,
// mas Singleton evita instanciar o serviço repetidamente.
builder.Services.AddSingleton<IMensageriaServico, RabbitMqMensageriaServico>();


// 8. Mapeador
builder.Services.AddAutoMapper(typeof(MapeadorRetaguarda));



// 9. Swagger e Autenticação
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Hiper ERP - API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT gerado no ADM"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var validIssuersConfig = builder.Configuration.GetSection("Jwt:ValidIssuers").Get<List<string>>();
        var validIssuers = validIssuersConfig ?? new List<string> { builder.Configuration["Jwt:Issuer"] ?? "Hiper.Erp" };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = validIssuers,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada no appsettings.json."))),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseMiddleware<TokenStorageMiddleware>();

app.UseMiddleware<TenantMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
