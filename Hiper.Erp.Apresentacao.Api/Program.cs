using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Hiper.Erp.Aplicacao.Mapeadores;
using Hiper.Erp.Aplicacao.Servicos;
using Hiper.Erp.Aplicacao.Servicos.Agentes;
using Hiper.Erp.Apresentacao.Api.Middlewares;
using Hiper.Erp.Infraestrutura.Bancos;
using Hiper.Erp.Infraestrutura.Bancos.SGDBs.Fabricas;
using Hiper.Erp.Infraestrutura.Cache;
using Hiper.Erp.Infraestrutura.Repositorios.Agentes;
using Hiper.Erp.Infraestrutura.Repositorios.ServicoExternos.Tenant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;




var builder = WebApplication.CreateBuilder(args);

// 1. Configurações de Infraestrutura Base
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();


// 2. Mock do Servidor de Administração (Centralizado)
builder.Services.AddScoped<IServicoAdministrador, ServicoAdministradorMock>();

// 3. Contexto do Tenant (Scoped para cada requisição)
builder.Services.AddScoped<ITenantContext, TenantContext>();

// 4. Configuração Dinâmica do DbContext (Multi-Tenancy)
builder.Services.AddScoped<RetaguardaDbContext>(provider =>
{
    var tenantContext = provider.GetRequiredService<ITenantContext>();

    // Se não houver conexão configurada (ex: rota pública ou erro no middleware), usa uma padrão
    if (string.IsNullOrEmpty(tenantContext.ConnectionString))
    {
        // Fallback para banco padrão ou erro controlado
        return FabricaConexoes.CriarContexto(
            Hiper.Erp.Dominio.Enumeradores.EnumTipoSgdb.SQLServer,
             "Data Source=localhost;Initial Catalog=idSaasErpDb; User ID=SA; Password=Y5hAmR9cJNKmGeY;TrustServerCertificate=True;");
    }

    // Cria o contexto dinamicamente baseado no Tenant resolvido pelo Middleware
    return FabricaConexoes.CriarContexto(tenantContext.TipoSgdb, tenantContext.ConnectionString);
});


// 5. UnitOfWork e Repositórios (Agora recebem o DbContext injetado)
builder.Services.AddScoped<IRepositorioAgentes, RepositorioAgentesDb>();


// 6. Serviços de Aplicação
builder.Services.AddScoped<IServicoAgentes, ServicoAgentes>();
//builder.Services.AddScoped<IServicoProdutos, ServicoProdutos>();


// 7. Mapeador
builder.Services.AddAutoMapper(typeof(MapeadorRetaguarda));



// 8. Swagger e Autenticação
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Hiper ERP - API", Version = "v1" });

    options.AddSecurityDefinition("TenantId", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "X-Tenant-Id",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Insira o ID do Tenant (ex: cliente_pg ou cliente_sql)"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "TenantId"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "ChaveMestraSuperSecreta123!")),
            ValidateLifetime = true,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});




var app = builder.Build();

// Middlewares Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

// 9. Middleware de Tenant (Deve vir ANTES da Autenticação para configurar o banco)
app.UseMiddleware<TenantMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
