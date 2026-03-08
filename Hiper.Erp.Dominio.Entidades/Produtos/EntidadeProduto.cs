using Hiper.Erp.Dominio.Atributos;
using Hiper.Erp.Dominio.Enumeradores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiper.Erp.Dominio.Entidades.Produtos
{
    [Table("T_PRODUTOS")]
    [VersaoTabela(EnumVersao.V1_1, Descricao = "Cadastro de Produtos")]
    public class EntidadeProduto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CODIGO")]
        public int Codigo { get; set; }

        [Column("NOME")]
        public string? Nome { get; set; }

        [Column("DESCRICAO")]
        public string? Descricao { get; set; }

        [Column("PRECO_VENDA")]
        public decimal? PrecoVenda { get; set; }

        [Column("ESTOQUE")]
        [VersaoCampo(EnumVersao.V1_2, Descricao = "Campo adicionado na versão 1.2 para controle de estoque")]
        public decimal? Estoque { get; set; }

        [Column("ATIVO")]
        public bool Ativo { get; set; }

        [Column("OBSERVACAO")]
        public string? Observacao { get; set; }
    }
}
