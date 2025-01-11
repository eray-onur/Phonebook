namespace Phonebook.Directory.Application.Models.Responses.ContactInfo
{
    public record DeleteContactInfoResponseModel
    {
        public Guid DeletedId { get; set; }
    }
}
