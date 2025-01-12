namespace Phonebook.Report.Application.Models.Responses.PersonList
{
    public record PersonGeneratedListResponseModel
    {
        public required string Location { get; set; }
        public required long PersonCount { get; set; }
        public required long PhoneCount { get; set; }
    }
}
