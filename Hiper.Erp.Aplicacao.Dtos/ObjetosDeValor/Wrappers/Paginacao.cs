namespace Hiper.Erp.Aplicacao.Dtos.ObjetosDeValor.Wrappers
{
    public class Paginacao
    {
        public Paginacao()
        {
        }

        public Paginacao(int paginaAtual, int quantidadeItensEmExibicao, int quantidadeItensTotal)
        {
            PaginaAtual = paginaAtual;
            QuantidadeItensPorPagina = quantidadeItensEmExibicao;
            QuantidadeItensTotal = quantidadeItensTotal;
        }

        public int PaginaAtual { get; set; }

        public int QuantidadeItensPorPagina { get; set; }

        public int QuantidadeItensTotal { get; set; }

    }
}
