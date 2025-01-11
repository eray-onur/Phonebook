using System.ComponentModel.DataAnnotations;

namespace Phonebook.Directory.Application.Models.Requests.ContactInfo
{
    public record AddContactInfoRequestModel
    {
        public Guid PersonId { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Location { get; set; }
        public string? Description { get; set; }
    }
}
