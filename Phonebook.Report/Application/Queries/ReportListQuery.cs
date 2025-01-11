using MediatR;

using Microsoft.EntityFrameworkCore;

using Phonebook.Report.Application.Models.Responses.Report;
using Phonebook.Report.Persistence;

namespace Phonebook.Report.Application.Queries
{
    public record ReportListQuery(): IRequest<ReportListResponseModel>;

    public class ReportListQueryHandler : IRequestHandler<ReportListQuery, ReportListResponseModel>
    {
        private readonly PhonebookDbContext phonebookContext;

        public ReportListQueryHandler(PhonebookDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<ReportListResponseModel> Handle(ReportListQuery request, CancellationToken cancellationToken)
        {
            var reportDbList = await phonebookContext.Set<Domain.PhonebookReport>().ToListAsync(cancellationToken);

            var result = new ReportListResponseModel();

            foreach (var reportDb in reportDbList)
            {
                var newReport = new ReportListItem
                {
                    Id = reportDb.Id,
                    Location = reportDb.Location,
                    RequestDate = reportDb.RequestDate,
                    ReportStatus = reportDb.ReportStatus,
                };
                result.ReportList.Add(newReport);
            }

            return result;
        }
    }
}
