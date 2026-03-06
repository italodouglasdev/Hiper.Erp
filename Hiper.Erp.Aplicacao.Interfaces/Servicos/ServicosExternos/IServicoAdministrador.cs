using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;
using Hiper.Erp.Aplicacao.Dtos.Wrappers;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos
{
    public interface IServicoAdministrador
    {
        Task<ResultadoOperacao<ConfiguracaoTenantDto>> ObtenhaConfiguracaoTenant(string tenantId);
    }

}
