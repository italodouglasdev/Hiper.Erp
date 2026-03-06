using Hiper.Erp.Dominio.Atributos;

namespace Hiper.Erp.Aplicacao.Dtos.Produtos
{
    public class DtoProduto
    {

        [ParametrosDeTabela(NomeColuna = "Código", ExibirColuna = true, ExibirFiltros = true)]
        public int Codigo { get; set; }

        [ParametrosDeTabela(NomeColuna = "Nome", ExibirColuna = true, ExibirFiltros = true)]
        public string? Nome { get; set; }

        [ParametrosDeTabela(NomeColuna = "Descrição", ExibirColuna = true, ExibirFiltros = true)]
        public string? Descricao { get; set; }

        [ParametrosDeTabela(NomeColuna = "Preço de Venda", ExibirColuna = true, ExibirFiltros = true)]
        public decimal? PrecoVenda { get; set; }

        [ParametrosDeTabela(NomeColuna = "Ativo", ExibirColuna = true, ExibirFiltros = true)]
        public bool Ativo { get; set; }

        [ParametrosDeTabela(NomeColuna = "Observação", ExibirColuna = false, ExibirFiltros = true)]
        public string? Observacao { get; set; }
    }
}
