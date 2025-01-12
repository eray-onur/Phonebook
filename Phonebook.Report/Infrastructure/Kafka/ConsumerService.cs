using Confluent.Kafka;

using MediatR;

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
            consumer.Subscribe(KafkaConstants.Topics.PersonListGenerated);

            return Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    var message = consumeResult.Message.Value;

                    // Decode the JSON string if it contains Unicode escape sequences
                    string decodedJson = System.Text.RegularExpressions.Regex.Unescape(message);

                    logger.LogInformation(decodedJson);

                    var payload = JsonSerializer.Deserialize<PersonGeneratedListResponseModel>(decodedJson);
                    if (payload != null)
                    {
                        mediator.Publish(
                            new PersonListGeneratedEvent(payload.Location, payload.PhoneCount, payload.PersonCount),
                            stoppingToken
                        );
                    }

                    logger.LogInformation($"Received person & contact list request: {message}");

                    _ = Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                consumer.Close();
            }, stoppingToken);
        }
    }
}
