using MediatR;

using Phonebook.Report.Infrastructure.Kafka;

using System.Text.Json;

namespace Phonebook.Report.Application.Events
{
    public record PersonListGeneratedEvent(string Location, long PhoneCount, long PersonCount) : INotification;

    public class PersonListGeneratedEventHandler : INotificationHandler<PersonListGeneratedEvent>
    {
        private readonly ProducerService producerService;
        public async Task Handle(PersonListGeneratedEvent notification, CancellationToken cancellationToken)
        {
            await producerService.ProduceAsync(KafkaConstants.Topics.PersonListGenerated, JsonSerializer.Serialize(notification));
            return;
        }
    }
}
