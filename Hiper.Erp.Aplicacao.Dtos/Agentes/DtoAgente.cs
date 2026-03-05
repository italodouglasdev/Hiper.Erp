using Hiper.Erp.Dominio.Atributos;

namespace Hiper.Erp.Aplicacao.Dtos.Agentes
{
    public class DtoAgente
    {

        [ParametrosDeTabela(NomeColuna = "Código", ExibirColuna = true, ExibirFiltros = true)]
        public int Codigo { get; set; }

        [ParametrosDeTabela(NomeColuna = "Cliente", ExibirColuna = false, ExibirFiltros = true)]
        public bool AgenteCliente { get; set; }

        [ParametrosDeTabela(NomeColuna = "Colaborador", ExibirColuna = false, ExibirFiltros = true)]
        public bool AgenteColaborador { get; set; }

        [ParametrosDeTabela(NomeColuna = "Tipo", ExibirColuna = false, ExibirFiltros = true)]
        public string? Tipo { get; set; }

        [ParametrosDeTabela(NomeColuna = "CNPJ/CPF", ExibirColuna = true, ExibirFiltros = true)]
        public string? CnpjCpf { get; set; }

        [ParametrosDeTabela(NomeColuna = "Razão/Nome", ExibirColuna = true, ExibirFiltros = true)]
        public string? RazaoNome { get; set; }

        [ParametrosDeTabela(NomeColuna = "Nome/Apelido", ExibirColuna = true, ExibirFiltros = true)]
        public string? FantasiaApelido { get; set; }

        [ParametrosDeTabela(NomeColuna = "Ativo", ExibirColuna = true, ExibirFiltros = true)]
        public bool Ativo { get; set; }

        [ParametrosDeTabela(NomeColuna = "Observação", ExibirColuna = false, ExibirFiltros = true)]
        public string? Observacao { get; set; }

    }
}
