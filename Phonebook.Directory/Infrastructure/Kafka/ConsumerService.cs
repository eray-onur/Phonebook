using Confluent.Kafka;

using MediatR;

using Phonebook.Directory.Application.Events;

using System.Text.Json;

using static Confluent.Kafka.ConfigPropertyNames;

namespace Phonebook.Directory.Infrastructure.Kafka
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> consumer;
        private ILogger<ConsumerService> logger;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public ConsumerService(ILogger<ConsumerService> logger, IConfiguration configuration, IMediator mediator)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.mediator = mediator;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = KafkaConstants.DirectoryConsumerGroup,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            consumer.Subscribe(KafkaConstants.Topics.PersonListGenerate);

            return Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    var message = consumeResult.Message.Value;
                    message = message.Trim('"');

                    // Decode the JSON string if it contains Unicode escape sequences
                    string decodedJson = System.Text.RegularExpressions.Regex.Unescape(message);

                    logger.LogInformation(decodedJson);

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    var reportEvent = JsonSerializer.Deserialize<PersonListGenerateEvent>(decodedJson, options);
                    if (reportEvent != null)
                    {
                        mediator.Publish(reportEvent, stoppingToken);
                    }
                    else
                    {
                        logger.LogInformation($"Failed to serialize message: {message}");
                    }

                    logger.LogInformation($"Received person & contact list request: {message}");

                    _ = Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                consumer.Close();
            }, stoppingToken);
        }
    }
}
