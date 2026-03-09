using Hiper.Erp.Aplicacao.Interfaces.Servicos.Mensageria;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Hiper.Erp.Infraestrutura.Mensageria
{
    /// <summary>
    /// Implementação concreta do serviço de mensageria utilizando RabbitMQ.
    /// 
    /// Esta classe é responsável por:
    /// 1. Estabelecer conexão com o servidor RabbitMQ usando as configurações do appsettings.json
    /// 2. Declarar a fila (caso não exista) de forma idempotente
    /// 3. Serializar o objeto em JSON e publicar na fila especificada
    /// 4. Garantir que a mensagem seja durável (persistida em disco pelo RabbitMQ)
    /// 
    /// Utiliza o padrão de "fire and forget" para não bloquear o fluxo principal da aplicação.
    /// Em caso de falha na publicação, o erro é logado mas NÃO impede o cadastro do cliente.
    /// </summary>
    public class RabbitMqMensageriaServico : IMensageriaServico
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqMensageriaServico> _logger;

        public RabbitMqMensageriaServico(IConfiguration configuration, ILogger<RabbitMqMensageriaServico> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Publica uma mensagem na fila do RabbitMQ.
        /// 
        /// Fluxo:
        /// 1. Lê as configurações de conexão do appsettings.json (seção "RabbitMQ")
        /// 2. Cria uma ConnectionFactory com os dados de Host, Porta, Usuário e Senha
        /// 3. Abre uma conexão assíncrona e cria um canal (channel)
        /// 4. Declara a fila como durável (durable: true) para que sobreviva a reinícios do RabbitMQ
        /// 5. Serializa o objeto genérico T em JSON (UTF-8)
        /// 6. Configura a mensagem como persistente (DeliveryMode = DeliveryModes.Persistent)
        /// 7. Publica a mensagem na fila usando o routingKey igual ao nome da fila
        /// </summary>
        public async Task PublicarMensagemAsync<T>(string nomeFila, T mensagem)
        {
            try
            {
                // Lê as configurações do RabbitMQ a partir do appsettings.json
                var hostName = _configuration["RabbitMQ:HostName"] ?? "localhost";
                var port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672");
                var userName = _configuration["RabbitMQ:UserName"] ?? throw new InvalidOperationException("RabbitMQ:UserName não configurado no appsettings.json.");
                var password = _configuration["RabbitMQ:Password"] ?? throw new InvalidOperationException("RabbitMQ:Password não configurado no appsettings.json.");

                // Cria a fábrica de conexões com os parâmetros configurados
                var factory = new ConnectionFactory
                {
                    HostName = hostName,
                    Port = port,
                    UserName = userName,
                    Password = password
                };

                // Abre conexão e canal de comunicação com o RabbitMQ (async no v7)
                await using var connection = await factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();

                // Declara a fila de forma idempotente (se já existir, não faz nada)
                // durable: true  -> a fila sobrevive a reinícios do RabbitMQ
                // exclusive: false -> a fila pode ser acessada por múltiplas conexões
                // autoDelete: false -> a fila NÃO é deletada quando o último consumidor desconecta
                await channel.QueueDeclareAsync(
                    queue: nomeFila,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                // Serializa o objeto em JSON com formatação legível para facilitar debug
                var jsonMensagem = JsonSerializer.Serialize(mensagem, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Converte a string JSON em bytes UTF-8 para envio
                var body = Encoding.UTF8.GetBytes(jsonMensagem);

                // Configura as propriedades da mensagem
                var properties = new BasicProperties
                {
                    // Persistent = a mensagem é salva em disco pelo RabbitMQ (não se perde se o servidor reiniciar)
                    DeliveryMode = DeliveryModes.Persistent,
                    ContentType = "application/json",
                    ContentEncoding = "utf-8"
                };

                // Publica a mensagem na fila
                // exchange: "" (string vazia) -> usa o exchange padrão (default exchange)
                // routingKey: nomeFila -> no exchange padrão, o routingKey é o próprio nome da fila
                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: nomeFila,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );

                _logger.LogInformation(
                    "[Mensageria] Mensagem publicada com sucesso na fila '{NomeFila}'. Conteúdo: {Mensagem}",
                    nomeFila,
                    jsonMensagem
                );
            }
            catch (Exception ex)
            {
                // Em caso de erro, apenas loga. NÃO propaga a exceção para não impedir o cadastro do cliente.
                // A mensageria é um processo auxiliar e não deve comprometer a operação principal.
                _logger.LogError(
                    ex,
                    "[Mensageria] Erro ao publicar mensagem na fila '{NomeFila}'. Detalhes: {Erro}",
                    nomeFila,
                    ex.Message
                );
            }
        }
    }
}
