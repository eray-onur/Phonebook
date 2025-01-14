using MediatR;

using Phonebook.Report.Application.Events;
using Phonebook.Report.Application.Models.Requests.Report;
using Phonebook.Report.Application.Models.Responses.Report;
using Phonebook.Report.Infrastructure.Kafka;
using Phonebook.Report.Persistence;

using System.Text.Json;

namespace Phonebook.Report.Application.Commands.Report
{
    public record CreateReportCommand(CreateReportRequestModel model): IRequest<CreateReportResponseModel>;
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, CreateReportResponseModel>
    {
        private readonly ReportDbContext phonebookContext;
        private readonly ProducerService producerService;

        public CreateReportCommandHandler(ReportDbContext phonebookContext, ProducerService producerService)
        {
            this.phonebookContext = phonebookContext;
            this.producerService = producerService;
        }

        public async Task<CreateReportResponseModel> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var newPerson = new Domain.PhonebookReport
            {
                Id = Guid.NewGuid(),
                ReportStatus = Domain.ReportStatus.QUEUED,
                RequestDate = DateTime.UtcNow,
                Location = request.model.Location
            };
            await phonebookContext.AddAsync(
                newPerson
            );

            var addResult = await phonebookContext.SaveChangesAsync(cancellationToken);

            Guid insertedId = (addResult <= 0) ? Guid.Empty : newPerson.Id;
            if (insertedId.Equals(Guid.Empty) == false)
            {
                await producerService.ProduceAsync(
                    KafkaConstants.Topics.PersonListGenerate, 
                    JsonSerializer.Serialize(new PersonListGenerateEvent(insertedId, request.model.Location))
                );
            }

            return new CreateReportResponseModel { CreatedId = insertedId };
        }
    }
}
