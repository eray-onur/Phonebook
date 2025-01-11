using System.Text.Json.Serialization;

namespace Phonebook.Directory.Application.Models.Requests.Person
{
    public record DeletePersonRequestModel
    {
        public Guid Id { get; set; }
    }
}
