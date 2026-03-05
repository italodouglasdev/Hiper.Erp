namespace Hiper.Erp.Aplicacao.Dtos.Filtros
{
    public class DtoFiltro
    {
        public DtoFiltro()
        {
            this.Campos = new List<DtoFiltroCampo>();
            this.Paginacao = new DtoFiltroPaginacao(1, 10);
        }

        public List<DtoFiltroCampo> Campos { get; set; }

        public DtoFiltroPaginacao Paginacao { get; set; }

    }
}
