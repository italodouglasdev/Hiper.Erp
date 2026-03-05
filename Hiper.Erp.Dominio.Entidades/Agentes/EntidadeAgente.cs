using Hiper.Erp.Dominio.Atributos;
using Hiper.Erp.Dominio.Enumeradores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiper.Erp.Dominio.Entidades.Agentes
{
    [Table("T_AGENTES")]
    [VersaoTabela(EnumVersao.V1_1, Descricao = "Cadastro de Agentes")]
    public class EntidadeAgente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CODIGO")]   
        public int Codigo { get; set; }

        [Column("AGENTE_CLIENTE")]    
        public bool AgenteCliente { get; set; }

        [Column("AGENTE_COLABORADOR")]
        public bool AgenteColaborador { get; set; }

        [Column("TIPO")]   
        public string? Tipo { get; set; }

        [Column("CNPJ_CPF")]       
        public string? CnpjCpf { get; set; }

        [Column("RAZAO_NOME")]        
        public string? RazaoNome { get; set; }

        [Column("FANTASIA_APELIDO")]       
        public string? FantasiaApelido { get; set; }

        [Column("ATIVO")]
        public bool Ativo { get; set; }

        [Column("OBSERVACAO")]       
        public string? Observacao { get; set; }
    }


}
