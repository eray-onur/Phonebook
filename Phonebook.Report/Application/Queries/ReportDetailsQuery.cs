using MediatR;

using Microsoft.EntityFrameworkCore;

using Phonebook.Report.Application.Models;
using Phonebook.Report.Application.Models.Requests.Report;
using Phonebook.Report.Application.Models.Responses.Report;
using Phonebook.Report.Persistence;

namespace Phonebook.Report.Application.Queries
{
    public record ReportDetailsQuery(ReportDetailsRequestModel model) : IRequest<ReportDetailsResponseModel>;

    public class ReportDetailsQueryHandler : IRequestHandler<ReportDetailsQuery, ReportDetailsResponseModel>
    {
        private readonly ReportDbContext phonebookContext;

        public ReportDetailsQueryHandler(ReportDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<ReportDetailsResponseModel> Handle(ReportDetailsQuery request, CancellationToken cancellationToken)
        {
            var existingReport = await phonebookContext.Set<Domain.PhonebookReport>()
                .Where(r => r.Id.Equals(request.model.Id))
                .FirstOrDefaultAsync(cancellationToken);
            if (existingReport == null)
                throw new Exception(ErrorMessages.FailedToFindReport);

            var result = new ReportDetailsResponseModel
            {
                Id = existingReport.Id,
                Location = existingReport.Location,
                RequestDate = existingReport.RequestDate,
                ReportStatus = existingReport.ReportStatus,
                PersonCount = existingReport.PersonCount,
                PhoneCount = existingReport.PhoneCount
            };
            return result;
        }
    }
}
