namespace Phonebook.Directory.Application.Models.Responses.Person
{
    public record DeletePersonResponseModel
    {
        public Guid DeletedId { get; set; }
    }
}
