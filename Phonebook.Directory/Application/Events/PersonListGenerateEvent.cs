using MediatR;

using Microsoft.EntityFrameworkCore;

using Phonebook.Directory.Application.Models.Responses.PersonList;
using Phonebook.Directory.Infrastructure.Kafka;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Application.Events
{
    public record PersonListGenerateEvent(Guid ReportId, string Location) : INotification;

    public class PersonListGenerateEventHandler : INotificationHandler<PersonListGenerateEvent>
    {
        private readonly ProducerService producerService;
        private readonly DirectoryDbContext phonebookDbContext;

        public PersonListGenerateEventHandler(DirectoryDbContext phonebookDbContext, ProducerService producerService)
        {
            this.producerService = producerService;
            this.phonebookDbContext = phonebookDbContext;
        }
        public async Task Handle(PersonListGenerateEvent notification, CancellationToken cancellationToken)
        {
            var contactInfos = await phonebookDbContext.ContactInfos
                .Where(ci => ci.Location.Equals(notification.Location))
                .ToListAsync(cancellationToken);

            long phoneCount = contactInfos.GroupBy(ci => ci.PhoneNumber).Count();
            long personCount = contactInfos.GroupBy(ci => ci.PersonId).Count();

            var payload = new PersonGeneratedListResponseModel { ReportId = notification.ReportId, Location = notification.Location, PersonCount = personCount, PhoneCount = phoneCount };

            await producerService.ProduceAsync(KafkaConstants.Topics.PersonListGenerated, payload);

            return;
        }

    }
}
