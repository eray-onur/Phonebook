﻿using MediatR;

using Microsoft.AspNetCore.Mvc;

using Phonebook.Directory.Application.Models.Requests.Person;
using Phonebook.Directory.Application.Models.Responses.Person;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Application.Commands.Person
{
    public record AddPersonCommand(AddPersonRequestModel Model) : IRequest<AddPersonResponseModel>;

    public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, AddPersonResponseModel>
    {
        private readonly DirectoryDbContext phonebookContext;

        public AddPersonCommandHandler(DirectoryDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<AddPersonResponseModel> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
            var newPerson = new Domain.Person
            {
                Id = Guid.NewGuid(),
                Name = request.Model.Name,
                LastName = request.Model.LastName,
                CompanyName = request.Model.CompanyName
            };
            await phonebookContext.AddAsync(
                newPerson
            );

            var addResult = await phonebookContext.SaveChangesAsync(cancellationToken);

            Guid insertedId = (addResult <= 0) ? Guid.Empty : newPerson.Id;

            return new AddPersonResponseModel { CreatedId =  insertedId };
        }
    }
}
