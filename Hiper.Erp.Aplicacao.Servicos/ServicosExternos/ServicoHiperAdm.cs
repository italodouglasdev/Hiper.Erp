using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;

namespace Hiper.Erp.Aplicacao.Servicos.ServicosExternos
{
    public class ServicoHiperAdm : ServicosBaseApiRest, IServicoAdministrador
    {

        private readonly ServicosBaseApiRest servicoApi;

        public ServicoHiperAdm(HttpClient httpClient) : base(httpClient)
        {
            servicoApi = new ServicosBaseApiRest(httpClient);
        }

        public async Task<ResultadoOperacao<ConfiguracaoTenantDto>> Login(string email, string senha)
        {
            var login = new LoginDto { Email = email, Password = senha };

            return await servicoApi.PostAsync<LoginDto, ConfiguracaoTenantDto>($"api/Auth/Login", login);
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
