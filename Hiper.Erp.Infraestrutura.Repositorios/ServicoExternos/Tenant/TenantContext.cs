using Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos;
using Hiper.Erp.Dominio.Enumeradores;

namespace Hiper.Erp.Infraestrutura.Repositorios.ServicoExternos.Tenant
{
    public class TenantContext : ITenantContext
    {
        public string TenantId { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public EnumTipoSgdb TipoSgdb { get; set; }
    }
}
