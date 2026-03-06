using Hiper.Erp.Aplicacao.Interfaces.Repositorios.Agentes;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.Agentes;
using Hiper.Erp.Aplicacao.Mapeadores;
using Hiper.Erp.Aplicacao.Servicos.Agentes;
using Hiper.Erp.Apresentacao.Web;
using Hiper.Erp.Infraestrutura.Repositorios.API.Agentes;
using idSaas.Erp.InterfaceUsuarios.RetaguardaWeb.Servicos;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ModalServico>();

builder.Services.AddAutoMapper(typeof(MapeadorRetaguarda));

builder.Services.AddScoped<IRepositorioAgentes, RepositorioAgentesApi>();
builder.Services.AddScoped<IServicoAgentes, ServicoAgentes>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7126")
});

await builder.Build().RunAsync();