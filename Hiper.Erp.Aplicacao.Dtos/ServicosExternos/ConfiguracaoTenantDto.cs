using Hiper.Erp.Dominio.Enumeradores;

namespace Hiper.Erp.Aplicacao.Dtos.ServicosExternos
{
    public class ConfiguracaoTenantDto
    {
        public string ConnectionString { get; set; }
        public EnumTipoSgdb TipoSgdb { get; set; }
    }
}
