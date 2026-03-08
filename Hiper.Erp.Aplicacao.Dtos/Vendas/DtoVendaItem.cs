using Hiper.Erp.Dominio.Atributos;

namespace Hiper.Erp.Aplicacao.Dtos.Vendas
{
    public class DtoVendaItem
    {

        [ParametrosDeTabela(NomeColuna = "Código", ExibirColuna = true, ExibirFiltros = true)]
        public int Codigo { get; set; }

        [ParametrosDeTabela(NomeColuna = "Código da Venda", ExibirColuna = true, ExibirFiltros = false)]
        public int CodigoVenda { get; set; }

        [ParametrosDeTabela(NomeColuna = "Código do Produto", ExibirColuna = true, ExibirFiltros = false)]
        public int CodigoProduto { get; set; }

        [ParametrosDeTabela(NomeColuna = "Produto", ExibirColuna = true, ExibirFiltros = true)]
        public string? NomeProduto { get; set; }

        [ParametrosDeTabela(NomeColuna = "Quantidade", ExibirColuna = false, ExibirFiltros = false)]
        public decimal? Quantidade { get; set; }

        [ParametrosDeTabela(NomeColuna = "Valor Unitário", ExibirColuna = false, ExibirFiltros = false)]
        public decimal? ValorUnitario { get; set; }

        [ParametrosDeTabela(NomeColuna = "Valor Total", ExibirColuna = false, ExibirFiltros = false)]
        public decimal? ValorTotal { get; set; }

    }
}
