using Phonebook.Report.Domain;

namespace Phonebook.Report.Application.Models.Requests.Report
{
    public record CreateReportRequestModel
    {
        public required string Location { get; set; }
    }
}
