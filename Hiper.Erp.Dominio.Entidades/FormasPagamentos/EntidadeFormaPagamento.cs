using Hiper.Erp.Dominio.Atributos;
using Hiper.Erp.Dominio.Enumeradores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiper.Erp.Dominio.Entidades.FormasPagamentos
{
    [Table("T_FORMAS_PAGAMENTO")]
    [VersaoTabela(EnumVersao.V1_1, Descricao = "Formas de Pagamento")]
    public class EntidadeFormaPagamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CODIGO")]
        [ParametrosDeTabela(NomeColuna = "Código", ExibirColuna = true, ExibirFiltros = true)]
        public int Codigo { get; set; }

        [Column("NOME")]
        [ParametrosDeTabela(NomeColuna = "Nome", ExibirColuna = false, ExibirFiltros = true)]
        public string? Nome { get; set; }

        [Column("DESCRICAO")]
        public string? Descricao { get; set; }
    }
}
