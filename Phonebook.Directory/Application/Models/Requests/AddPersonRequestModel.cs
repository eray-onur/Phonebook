﻿namespace Phonebook.Directory.Application.Models.Requests
{
    public record AddPersonRequestModel
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? CompanyName { get; set; }
    }
}
