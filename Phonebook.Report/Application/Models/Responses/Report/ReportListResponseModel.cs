using Phonebook.Report.Domain;

namespace Phonebook.Report.Application.Models.Responses.Report
{
    public class ReportListResponseModel
    {
        public List<ReportListItem> ReportList { get; set; } = new List<ReportListItem>();
    }

    public record ReportListItem
    {
        public required Guid Id { get; set; }
        public required DateTime RequestDate { get; set; }
        public required ReportStatus ReportStatus { get; set; }
        public required string Location { get; set; }
    }
}
