using Hiper.Erp.Dominio.Enumeradores;

namespace idSaas.Erp.Aplicacao.Dtos.Tabelas
{
    public class FiltroCampo
    {
        public EnumTipoFiltroQuery TipoFiltro { get; set; }
        public string? Valor { get; set; }
    }
}
