using Hiper.Erp.Dominio.Enumeradores;

namespace Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros
{
    public class DtoFiltroCampoValor
    {
        public string Valor { get; set; }
        public EnumTipoFiltroQuery TipoFiltro { get; set; }

    }
}
