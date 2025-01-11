namespace Phonebook.Directory.Application.Models.Responses.Person
{
    public record PersonListResponseModel
    {
        public List<PersonListItem> PersonList { get; set; } = new List<PersonListItem>();
    }

    public record PersonListItem
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? CompanyName { get; set; }
    }
}
