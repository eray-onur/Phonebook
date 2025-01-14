using MediatR;

using Microsoft.EntityFrameworkCore;

using Phonebook.Report.Infrastructure.Kafka;
using Phonebook.Report.Persistence;

using System.Text.Json;

namespace Phonebook.Report.Application.Events
{
    public record PersonListGeneratedEvent(Guid ReportId, string Location, long PhoneCount, long PersonCount) : INotification;

    public class PersonListGeneratedEventHandler : INotificationHandler<PersonListGeneratedEvent>
    {
        private readonly PhonebookDbContext phonebookContext;
        public PersonListGeneratedEventHandler(PhonebookDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }
        public async Task Handle(PersonListGeneratedEvent notification, CancellationToken cancellationToken)
        {
            var existingReport = await phonebookContext.Set<Domain.PhonebookReport>()
                .Where(r => r.Id == notification.ReportId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingReport == null)
                return;

            existingReport.Location = notification.Location;
            existingReport.PhoneCount = notification.PhoneCount;
            existingReport.PersonCount = notification.PersonCount;
            existingReport.ReportStatus = Domain.ReportStatus.FINISHED;


            phonebookContext.Update(existingReport);
            var result = await phonebookContext.SaveChangesAsync(cancellationToken);

        }
    }
}
