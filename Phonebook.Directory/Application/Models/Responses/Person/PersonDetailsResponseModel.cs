namespace Phonebook.Directory.Application.Models.Responses.Person
{
    public record PersonDetailsResponseModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? CompanyName { get; set; }
        public List<PersonContactInfoDetailsItem> ContactInfos { get; set; } = new List<PersonContactInfoDetailsItem>();
    }
    public record PersonContactInfoDetailsItem
    {
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Location { get; set; }
        public string? Description { get; set; }
    }
}
