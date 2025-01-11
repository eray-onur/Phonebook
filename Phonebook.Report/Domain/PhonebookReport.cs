namespace Phonebook.Report.Domain
{
    public class PhonebookReport
    {
        public required Guid Id { get; set; }
        public required DateTime RequestDate { get; set; }
        public required ReportStatus ReportStatus { get; set; }
        public required string Location { get; set; }
        public long? PersonCount { get; set; }
        public long? PhoneCount { get; set; }
    }

    public enum ReportStatus
    {
        QUEUED,
        FINISHED,
        FAILED
    }
}
