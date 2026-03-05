using Hiper.Erp.Dominio.Atributos;
using Hiper.Erp.Dominio.Enumeradores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiper.Erp.Dominio.Entidades.Vendas
{
    [Table("T_VENDAS_ITENS")]
    [VersaoTabela(EnumVersao.V1_1, Descricao = "Itens das Vendas")]
    public class EntidadeVendaItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CODIGO")]
        public int Codigo { get; set; }

        [Column("CODIGO_VENDA")]
        public int CodigoVenda { get; set; }

        [Column("CODIGO_PRODUTO")]
        public int CodigoProduto { get; set; }

        [Column("NOME_PRODUTO")]
        public string? NomeProduto { get; set; }
    }
}
