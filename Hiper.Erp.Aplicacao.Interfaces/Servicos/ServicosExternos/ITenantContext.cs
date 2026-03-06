using Hiper.Erp.Dominio.Enumeradores;

namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.ServicosExternos
{
    public interface ITenantContext
    {
        string TenantId { get; set; }
        string ConnectionString { get; set; }
        EnumTipoSgdb TipoSgdb { get; set; }
    }
}
