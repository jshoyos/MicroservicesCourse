using System.Text;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) => 
            {
                _logger.LogInformation("Event Received");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProccesor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory() {HostName = _config["RabbitMQHost"], Port = int.Parse(_config["RabbitMQPort"])};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: string.Empty);

            _logger.LogInformation("Listening on the message bus");

            _connection.ConnectionShutdown += RabbitMQ_Connectionshutdown;
        }

        private void RabbitMQ_Connectionshutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("Connection shutdown");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }

        public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProccesor, ILogger<MessageBusSubscriber> logger)
        {
            _config = config;
            _eventProccesor = eventProccesor;
            _logger = logger;
            InitRabbitMQ();
        }

        private readonly IConfiguration _config;
        private readonly IEventProcessor _eventProccesor;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private string _queueName;
        private IConnection _connection;
        private IModel _channel;
    }
}