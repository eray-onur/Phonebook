using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Application.Models.Requests.Person;
using Phonebook.Directory.Application.Models.Responses.Person;
using Phonebook.Directory.Domain;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Application.Commands.Person
{
    public record UpdatePersonCommand(UpdatePersonRequestModel Model) : IRequest<UpdatePersonResponseModel>;

    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, UpdatePersonResponseModel>
    {
        private readonly PhonebookDbContext phonebookContext;

        public UpdatePersonCommandHandler(PhonebookDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<UpdatePersonResponseModel> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var existingPerson = await phonebookContext.Set<Domain.Person>().FirstOrDefaultAsync(p => p.Id.Equals(request.Model.Id));
            if (existingPerson == null)
            {
                throw new Exception(ErrorMessages.FailedToFindPerson);
            }

            existingPerson.Name = request.Model.Name;
            existingPerson.LastName = request.Model.LastName;
            existingPerson.CompanyName = request.Model.CompanyName;

            phonebookContext.Update(
                existingPerson
            );

            var addResult = await phonebookContext.SaveChangesAsync(cancellationToken);

            Guid updatedId = (addResult <= 0) ? Guid.Empty : existingPerson.Id;

            return new UpdatePersonResponseModel { UpdatedId = updatedId };
        }
    }
}
