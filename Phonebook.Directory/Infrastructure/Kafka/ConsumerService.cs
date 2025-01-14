using Confluent.Kafka;

using MediatR;

using Phonebook.Directory.Application.Events;
using Phonebook.Directory.Persistence;

using System.Text.Json;

using static Confluent.Kafka.ConfigPropertyNames;

namespace Phonebook.Directory.Infrastructure.Kafka
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> consumer;
        private ILogger<ConsumerService> logger;
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory scopeFactory;

        public ConsumerService(ILogger<ConsumerService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.configuration = configuration;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = KafkaConstants.DirectoryConsumerGroup,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            this.scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            consumer.Subscribe(KafkaConstants.Topics.PersonListGenerate);

            return Task.Run(async () =>
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
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                            await mediator.Publish(new PersonListGenerateEvent(reportEvent.ReportId, reportEvent.Location), stoppingToken);
                        }
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
