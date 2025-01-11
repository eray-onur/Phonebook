using System.Text.Json.Serialization;

namespace Phonebook.Directory.Application.Models.Requests
{
    public record DeletePersonRequestModel
    {
        public Guid Id { get; set; }
    }
}
