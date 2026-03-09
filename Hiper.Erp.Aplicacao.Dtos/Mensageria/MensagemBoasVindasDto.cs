namespace Hiper.Erp.Aplicacao.Dtos.Mensageria
{
    /// <summary>
    /// DTO que representa a mensagem de boas-vindas a ser enviada para a fila do RabbitMQ.
    /// Contém todas as informações necessárias para que um futuro consumidor
    /// possa processar o envio do e-mail de boas-vindas ao cliente.
    /// </summary>
    public class MensagemBoasVindasDto
    {
        /// <summary>
        /// Código identificador do agente/cliente cadastrado.
        /// </summary>
        public int CodigoAgente { get; set; }

        /// <summary>
        /// Razão social ou nome completo do agente/cliente.
        /// </summary>
        public string? RazaoNome { get; set; }

        /// <summary>
        /// Nome fantasia ou apelido do agente/cliente.
        /// </summary>
        public string? FantasiaApelido { get; set; }

        /// <summary>
        /// CNPJ ou CPF do agente/cliente.
        /// </summary>
        public string? CnpjCpf { get; set; }

        /// <summary>
        /// Tipo da ação que originou a mensagem (ex: "NovoCliente").
        /// </summary>
        public string Tipo { get; set; } = "BoasVindas";

        /// <summary>
        /// Data e hora em que a mensagem foi gerada (UTC).
        /// </summary>
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Mensagem descritiva do evento.
        /// </summary>
        public string Descricao { get; set; } = "Novo cliente cadastrado. Enviar e-mail de boas-vindas.";
    }
}
