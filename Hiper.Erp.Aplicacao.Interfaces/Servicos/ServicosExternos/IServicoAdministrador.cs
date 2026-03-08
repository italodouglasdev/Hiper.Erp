using Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers;
using Hiper.Erp.Aplicacao.Dtos.ServicosExternos;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos
{
    public interface IServicoAdministrador
    {
        Task<ResultadoOperacao<UsuarioLogadoDto>> Login(string email, string senha);
        Task<ResultadoOperacao<List<LojaDto>>> Lojas();
        Task<ResultadoOperacao<ConfiguracaoTenantDto>> ObtenhaConfiguracaoTenant(string tenantId);
    }

}
