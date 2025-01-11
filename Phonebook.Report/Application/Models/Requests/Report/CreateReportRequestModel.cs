using Phonebook.Report.Domain;

namespace Phonebook.Report.Application.Models.Requests.Report
{
    public record CreateReportRequestModel
    {
        public required string Location { get; set; }
        public long? PersonCount { get; set; }
        public long? PhoneCount { get; set; }
    }
}
