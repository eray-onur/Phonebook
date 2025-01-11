using MediatR;

using Phonebook.Report.Application.Models.Requests.Report;
using Phonebook.Report.Application.Models.Responses.Report;
using Phonebook.Report.Persistence;

namespace Phonebook.Report.Application.Commands.Report
{
    public record CreateReportCommand(CreateReportRequestModel model): IRequest<CreateReportResponseModel>;
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, CreateReportResponseModel>
    {
        private readonly PhonebookDbContext phonebookContext;

        public CreateReportCommandHandler(PhonebookDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
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

            return new CreateReportResponseModel { CreatedId = insertedId };
        }
    }
}
