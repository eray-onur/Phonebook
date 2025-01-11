namespace Phonebook.Directory.Application.Models.Responses.Person
{
    public record AddPersonResponseModel
    {
        public required Guid CreatedId { get; set; }
    }
}
