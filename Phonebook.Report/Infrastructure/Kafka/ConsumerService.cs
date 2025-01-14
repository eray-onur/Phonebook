using Confluent.Kafka;

using MediatR;

using Microsoft.Extensions.Options;

using Phonebook.Report.Application.Events;
using Phonebook.Report.Application.Models.Responses.PersonList;

using System.Text.Json;

using static Confluent.Kafka.ConfigPropertyNames;

namespace Phonebook.Report.Infrastructure.Kafka
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
            this.scopeFactory = scopeFactory;
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
            consumer.Subscribe(KafkaConstants.Topics.PersonListGenerated);

            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    var message = consumeResult.Message.Value;

                    // Decode the JSON string if it contains Unicode escape sequences
                    string decodedJson = System.Text.RegularExpressions.Regex.Unescape(message);

                    logger.LogInformation(decodedJson);

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var reportEvent = JsonSerializer.Deserialize<PersonListGeneratedEvent>(decodedJson, options);
                    if (reportEvent != null)
                    {
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            await mediator.Publish(reportEvent, stoppingToken); // You can use producerService here if needed
                        }
                    }

                    logger.LogInformation($"Received person & contact list request: {message}");

                    _ = Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                consumer.Close();
            }, stoppingToken);
        }
    }
}
