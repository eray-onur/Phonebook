namespace Phonebook.Directory.Domain
{
    public class ContactInfo
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Location { get; set; }
        public string? Description { get; set; }
    }
}
