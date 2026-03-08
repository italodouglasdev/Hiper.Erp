using Hiper.Erp.Dominio.Enumeradores;

namespace Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Tabelas
{
    public class FiltroCampo
    {
        public EnumTipoFiltroQuery TipoFiltro { get; set; }
        public string? Valor { get; set; }
    }
}
