using MediatR;
using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Persistence;
using Microsoft.EntityFrameworkCore;
using Phonebook.Directory.Application.Models.Requests.Person;
using Phonebook.Directory.Application.Models.Responses.Person;

namespace Phonebook.Directory.Application.Commands.Person
{
    public record DeletePersonCommand(DeletePersonRequestModel Model) : IRequest<DeletePersonResponseModel>;

    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, DeletePersonResponseModel>
    {
        private readonly PhonebookDbContext phonebookContext;

        public DeletePersonCommandHandler(PhonebookDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<DeletePersonResponseModel> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var existingPerson = await phonebookContext.Set<Domain.Person>().FirstOrDefaultAsync(p => p.Id.Equals(request.Model.Id));
            if (existingPerson == null)
            {
                throw new Exception(ErrorMessages.FailedToFindPerson);
            }


            phonebookContext.Remove(existingPerson);

            var addResult = await phonebookContext.SaveChangesAsync(cancellationToken);

            Guid deletedId = (addResult <= 0) ? Guid.Empty : existingPerson.Id;

            return new DeletePersonResponseModel { DeletedId = deletedId };
        }
    }
}
