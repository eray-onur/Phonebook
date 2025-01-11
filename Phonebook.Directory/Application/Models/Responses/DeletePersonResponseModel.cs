namespace Phonebook.Directory.Application.Models.Responses
{
    public record DeletePersonResponseModel
    {
        public Guid DeletedId { get; set; }
    }
}
