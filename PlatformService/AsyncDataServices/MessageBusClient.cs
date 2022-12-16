using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection.IsOpen) 
            {
                _logger.LogInformation("RabbitMQ connection open sending message");
                // Send the message
                SendMessage(message);
            }
            else
            {
                _logger.LogInformation("RabbitMQ connection not opened. Failed to send message");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                             routingKey: string.Empty,
                             basicProperties: null,
                             body: body);
            _logger.LogInformation($"Message has been sent: {message}");
        }

        public void Dispose() 
        {
            _logger.LogInformation("Message bus disposed");
            if (_channel.IsOpen) {
                _channel.Close();
                _connection.Close();
            }
        }

        public MessageBusClient(IConfiguration config, ILogger<MessageBusClient> logger)
        {
            int port;
            _config = config;
            _logger = logger;
            if (!int.TryParse(_config["RabbitMQPort"], out port)) port = 5672;

            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQHost"],
                Port = port
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                _logger.LogInformation("Connected to RabbitMQ channel");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("RabbitMQ connection shutdown");
        }

        private readonly IConfiguration _config;
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
    }
}