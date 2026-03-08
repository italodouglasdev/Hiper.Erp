using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.FormasPagamentos;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Produtos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Vendas;
using Hiper.Erp.Aplicacao.Mapeadores;
using Hiper.Erp.Aplicacao.Servicos.Agentes;
using Hiper.Erp.Aplicacao.Servicos.FormasPagamentos;
using Hiper.Erp.Aplicacao.Servicos.Produtos;
using Hiper.Erp.Aplicacao.Servicos.Vendas;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Hiper.Erp.Aplicacao.Servicos.ServicosExternos;
using Hiper.Erp.Apresentacao.Web;
using Hiper.Erp.Apresentacao.Web.Handlers;
using Hiper.Erp.Infraestrutura.Repositorios.API.Agentes;
using Hiper.Erp.Infraestrutura.Repositorios.API.FormasPagamentos;
using Hiper.Erp.Infraestrutura.Repositorios.API.Produtos;
using Hiper.Erp.Infraestrutura.Repositorios.API.Vendas;
using Hiper.Erp.InterfaceUsuarios.RetaguardaWeb.Servicos;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ModalServico>();
builder.Services.AddTransient<TenantHandler>();

builder.Services.AddAutoMapper(typeof(MapeadorRetaguarda));

builder.Services.AddScoped<IRepositorioAgentes, RepositorioAgentesApi>();
builder.Services.AddScoped<IRepositorioFormasPagamentos, RepositorioFormasPagamentosApi>();
builder.Services.AddScoped<IRepositorioProdutos, RepositorioProdutosApi>();
builder.Services.AddScoped<IRepositorioVendas, RepositorioVendasApi>();
builder.Services.AddScoped<IRepositorioVendasItens, RepositorioVendasItensApi>();

builder.Services.AddScoped<IServicoAgentes, ServicoAgentes>();
builder.Services.AddScoped<IServicoFormasPagamentos, ServicoFormasPagamentos>();
builder.Services.AddScoped<IServicoProdutos, ServicoProdutos>();
builder.Services.AddScoped<IServicoVendas, ServicoVendas>();
builder.Services.AddScoped<IServicoVendasItens, ServicoVendasItens>();

builder.Services.AddHttpClient("HiperAdm", client =>
{
    client.BaseAddress = new Uri("https://localhost:7125");
})
.AddHttpMessageHandler<TenantHandler>();

builder.Services.AddScoped<IServicoAdministrador, ServicoHiperAdm>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("HiperAdm");
    return new ServicoHiperAdm(httpClient);
});

builder.Services.AddHttpClient("HiperApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7126");
})
.AddHttpMessageHandler<TenantHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HiperApi"));

await builder.Build().RunAsync();