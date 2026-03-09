namespace Hiper.Erp.Aplicacao.Interfaces.Servicos.Mensageria
{
    /// <summary>
    /// Interface que define o contrato para publicação de mensagens em uma fila de mensageria.
    /// Segue o princípio de inversão de dependência (DIP), permitindo que a camada de aplicação
    /// dependa apenas da abstração, e não da implementação concreta (RabbitMQ, Azure Service Bus, etc.).
    /// </summary>
    public interface IMensageriaServico
    {
        /// <summary>
        /// Publica uma mensagem em uma fila específica de forma assíncrona.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto que será serializado e enviado como mensagem.</typeparam>
        /// <param name="nomeFila">Nome da fila onde a mensagem será publicada.</param>
        /// <param name="mensagem">Objeto que será serializado em JSON e enviado para a fila.</param>
        Task PublicarMensagemAsync<T>(string nomeFila, T mensagem);
    }
}
