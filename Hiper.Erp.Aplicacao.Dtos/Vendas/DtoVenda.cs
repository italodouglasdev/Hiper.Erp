using Hiper.Erp.Dominio.Atributos;

namespace Hiper.Erp.Aplicacao.Dtos.Vendas
{
    public class DtoVenda
    {
        [ParametrosDeTabela(NomeColuna = "Código", ExibirColuna = true, ExibirFiltros = true)]
        public int Codigo { get; set; }

        [ParametrosDeTabela(NomeColuna = "Código da Forma de Pagamento", ExibirColuna = true, ExibirFiltros = false)]
        public int CodigoFormaPagamento { get; set; }

        [ParametrosDeTabela(NomeColuna = "Forma de Pagamento", ExibirColuna = true, ExibirFiltros = true)]
        public string? FormaPagamento { get; set; }

        [ParametrosDeTabela(NomeColuna = "Código do Cliente", ExibirColuna = true, ExibirFiltros = false)]
        public int CodigoCliente { get; set; }

        [ParametrosDeTabela(NomeColuna = "Cliente", ExibirColuna = true, ExibirFiltros = true)]
        public string? NomeCliente { get; set; }

        [ParametrosDeTabela(NomeColuna = "Data/Hora", ExibirColuna = false, ExibirFiltros = true)]
        public DateTime? DataHora { get; set; }

        [ParametrosDeTabela(NomeColuna = "Descrição", ExibirColuna = false, ExibirFiltros = true)]
        public string? Descricao { get; set; }

    }
}
