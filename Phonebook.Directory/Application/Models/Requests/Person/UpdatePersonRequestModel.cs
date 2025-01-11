namespace Phonebook.Directory.Application.Models.Requests.Person
{
    public record UpdatePersonRequestModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? CompanyName { get; set; }
    }
}
