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

        [Column("QUANTIDADE")]
        [VersaoCampo(EnumVersao.V1_2, Descricao = "Campo adicionado na versão 1.2 para quantidade do item")]
        public decimal? Quantidade { get; set; }

        [Column("VALOR_UNITARIO")]
        [VersaoCampo(EnumVersao.V1_2, Descricao = "Campo adicionado na versão 1.2 para valor uniário do item")]
        public decimal? ValorUnitario { get; set; }

        [Column("VALOR_TOTAL")]
        [VersaoCampo(EnumVersao.V1_2, Descricao = "Campo adicionado na versão 1.2 para valor total do item")]
        public decimal? ValorTotal { get; set; }
    }
}
