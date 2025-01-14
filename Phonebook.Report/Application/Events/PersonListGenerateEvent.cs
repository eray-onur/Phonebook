using MediatR;

using Phonebook.Report.Infrastructure.Kafka;

using System.Text.Json;

namespace Phonebook.Report.Application.Events
{
    public record PersonListGenerateEvent(Guid ReportId, string Location) : INotification;

    public class PersonListGenerateEventHandler : INotificationHandler<PersonListGenerateEvent>
    {
        private readonly ProducerService producerService;
        public PersonListGenerateEventHandler(ProducerService producerService)
        {
            this.producerService = producerService;
        }
        public async Task Handle(PersonListGenerateEvent notification, CancellationToken cancellationToken)
        {
            await producerService.ProduceAsync(KafkaConstants.Topics.PersonListGenerate, JsonSerializer.Serialize(notification));
            return;
        }
    }
}
