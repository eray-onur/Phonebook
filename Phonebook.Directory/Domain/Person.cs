namespace Phonebook.Directory.Domain
{
    public class Person
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? CompanyName { get; set; }
    }
}
