using Hiper.Erp.Dominio.Atributos;
using Hiper.Erp.Dominio.Enumeradores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiper.Erp.Dominio.Entidades.Vendas
{

    [Table("T_VENDAS")]
    [VersaoTabela(EnumVersao.V1_1, Descricao = "Cadastro de Vendas")]
    public class EntidadeVenda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CODIGO")]
        public int Codigo { get; set; }

        [Column("CODIGO_FORMA_PAGAMENTO")]
        public int CodigoFormaPagamento { get; set; }

        [Column("FORMA_PAGAMENTO")]
        public string? FormaPagamento { get; set; }

        [Column("CODIGO_CLIENTE")]
        public int CodigoCliente { get; set; }

        [Column("NOME_CLIENTE")]
        public string? NomeCliente { get; set; }

        [Column("VALOR_TOTAL")]
        [VersaoCampo(EnumVersao.V1_2, Descricao = "Campo adicionado na versão 1.2 para cálculo do valor total da venda")]
        public decimal? ValorTotal { get; set; }

        [Column("DATA_HORA")]
        public DateTime? DataHora { get; set; }

        [Column("DESCRICAO")]
        public string? Descricao { get; set; }
    }
}
