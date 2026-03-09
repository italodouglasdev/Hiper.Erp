namespace Hiper.Erp.Aplicacao.Dtos.Mensageria
{
    public class MensagemBoasVindasDto
    {   
        public int CodigoAgente { get; set; }
        public string? RazaoNome { get; set; }        
        public string? FantasiaApelido { get; set; }    
        public string? CnpjCpf { get; set; }
        public string Tipo { get; set; } = "BoasVindas";
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;    
        public string Descricao { get; set; } = "Novo cliente cadastrado. Enviar e-mail de boas-vindas.";
    }
}
