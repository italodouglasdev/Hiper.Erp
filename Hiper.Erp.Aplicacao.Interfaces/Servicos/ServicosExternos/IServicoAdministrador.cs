using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos
{
    public interface IServicoAdministrador
    {
        Task<ResultadoOperacao<ConfiguracaoTenantDto>> ObtenhaConfiguracaoTenant(string tenantId);
    }

}
