namespace Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Filtros
{
    public class DtoFiltroPaginacao
    {
        public DtoFiltroPaginacao()
        {

        }

        public DtoFiltroPaginacao(int paginaAtual, int quantidadeItensEmExibicao)
        {
            PaginaAtual = paginaAtual;
            QuantidadeItensPorPagina = quantidadeItensEmExibicao;
        }

        public int PaginaAtual { get; set; }

        public int QuantidadeItensPorPagina { get; set; }

        public string? Ordenacao { get; set; }

        public string? Organizacao { get; set; }

    }
}
