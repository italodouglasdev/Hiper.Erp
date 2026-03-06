using Hiper.Erp.Dominio.Atributos;

namespace Hiper.Erp.Aplicacao.Dtos.FormasPagamentos
{
    public class DtoFormaPagamento
    {

        [ParametrosDeTabela(NomeColuna = "Código", ExibirColuna = true, ExibirFiltros = true)]
        public int Codigo { get; set; }

        [ParametrosDeTabela(NomeColuna = "Nome", ExibirColuna = true, ExibirFiltros = true)]
        public string? Nome { get; set; }

        [ParametrosDeTabela(NomeColuna = "Descrição", ExibirColuna = false, ExibirFiltros = true)]
        public string? Descricao { get; set; }

    }
}
