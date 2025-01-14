using Confluent.Kafka;

using System.Text.Json;

using static Confluent.Kafka.ConfigPropertyNames;

namespace Phonebook.Report.Infrastructure.Kafka
{
    public class ProducerService
    {
        private readonly IConfiguration configuration;
        private readonly IProducer<string, string> producer;

        public ProducerService(IConfiguration configuration)
        {
            this.configuration = configuration;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            producer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        public async Task ProduceAsync<T>(string topic, T message)
        {
            var payload = JsonSerializer.Serialize(message);
            var kafkamessage = new Message<string, string> { Value = payload };

            await producer.ProduceAsync(topic, kafkamessage);
        }
    }
}
