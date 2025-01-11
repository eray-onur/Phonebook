namespace Phonebook.Directory.Application.Models.Requests
{
    public class UpdatePersonRequestModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? CompanyName { get; set; }
    }
}
