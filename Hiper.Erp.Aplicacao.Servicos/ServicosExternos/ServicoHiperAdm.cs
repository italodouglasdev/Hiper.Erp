using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Microsoft.AspNetCore.Http;

namespace Hiper.Erp.Aplicacao.Servicos.ServicosExternos
{
    public class ServicoHiperAdm : ServicosBaseApiRest, IServicoAdministrador
    {

        private readonly ServicosBaseApiRest servicoApi;

        public ServicoHiperAdm(HttpClient httpClient) : base(httpClient, null)
        {
            servicoApi = new ServicosBaseApiRest(httpClient, null);
        }

        public ServicoHiperAdm(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
        {
            servicoApi = new ServicosBaseApiRest(httpClient, httpContextAccessor);
        }


        public async Task<ResultadoOperacao<UsuarioLogadoDto>> Login(string email, string senha)
        {
            var login = new LoginDto { Email = email, Password = senha };

            return await servicoApi.PostAsync<LoginDto, UsuarioLogadoDto>($"api/Auth/Login", login);
        }

        public async Task<ResultadoOperacao<List<LojaDto>>> Lojas()
        {
            return await servicoApi.GetAsync<List<LojaDto>>($"api/Auth/Lojas");
        }

        public async Task<ResultadoOperacao<ConfiguracaoTenantDto>> ObtenhaConfiguracaoTenant(string tenantId)
        {
            return await servicoApi.GetAsync<ConfiguracaoTenantDto>($"api/Tenants/ObtenhaConfiguracaoTenantPorXTenantId/{tenantId}");

        }

    }
}
