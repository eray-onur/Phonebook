using MediatR;

using Microsoft.EntityFrameworkCore;

using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Helpers;
using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Application.Models.Requests;
using Phonebook.Directory.Application.Models.Requests.ContactInfo;
using Phonebook.Directory.Application.Models.Responses;
using Phonebook.Directory.Application.Models.Responses.ContactInfo;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Application.Commands.ContactInfo
{
    public record AddContactInfoCommand(AddContactInfoRequestModel Model) : IRequest<AddContactInfoResponseModel>;

    public class AddContactInfoCommandHandler : IRequestHandler<AddContactInfoCommand, AddContactInfoResponseModel>
    {
        private readonly DirectoryDbContext phonebookContext;

        public AddContactInfoCommandHandler(DirectoryDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<AddContactInfoResponseModel> Handle(AddContactInfoCommand request, CancellationToken cancellationToken)
        {
            if(PhoneHelper.IsValidPhoneNumber(request.Model.PhoneNumber) == false)
                throw new CommandValidationException(ErrorMessages.InvalidPhoneNumber);

            var existingPerson = await phonebookContext.Set<Domain.Person>().FirstOrDefaultAsync(p => p.Id.Equals(request.Model.PersonId));
            if (existingPerson == null)
            {
                throw new Exception(ErrorMessages.FailedToFindPerson);
            }

            var newContactInfo = new Domain.ContactInfo
            {
                Id = Guid.NewGuid(),
                PersonId = request.Model.PersonId,
                Email = request.Model.Email,
                PhoneNumber = request.Model.PhoneNumber,
                Description = request.Model.Description,
                Location = request.Model.Location
            };

            await phonebookContext.AddAsync(
                newContactInfo
            );

            var addResult = await phonebookContext.SaveChangesAsync(cancellationToken);

            Guid insertedId = (addResult <= 0) ? Guid.Empty : newContactInfo.Id;

            return new AddContactInfoResponseModel { AddedContactInfoId = insertedId };
        }
    }
}
