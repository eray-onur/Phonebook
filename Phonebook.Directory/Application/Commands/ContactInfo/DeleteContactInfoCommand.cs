using MediatR;

using Microsoft.EntityFrameworkCore;

using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Application.Models.Requests.ContactInfo;
using Phonebook.Directory.Application.Models.Responses;
using Phonebook.Directory.Application.Models.Responses.ContactInfo;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Application.Commands.ContactInfo
{
    public record DeleteContactInfoCommand(DeleteContactInfoRequestModel Model) : IRequest<DeleteContactInfoResponseModel>;

    public class DeleteContactInfoCommandHandler : IRequestHandler<DeleteContactInfoCommand, DeleteContactInfoResponseModel>
    {
        private readonly PhonebookDbContext phonebookContext;

        public DeleteContactInfoCommandHandler(PhonebookDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<DeleteContactInfoResponseModel> Handle(DeleteContactInfoCommand request, CancellationToken cancellationToken)
        {
            var existingContactInfo = await phonebookContext.Set<Domain.ContactInfo>().FirstOrDefaultAsync(p => p.Id.Equals(request.Model.Id));
            if (existingContactInfo == null)
            {
                throw new Exception(ErrorMessages.FailedToFindContactInfo);
            }


            phonebookContext.Remove(existingContactInfo);

            var removeResult = await phonebookContext.SaveChangesAsync(cancellationToken);

            Guid deletedId = (removeResult <= 0) ? Guid.Empty : existingContactInfo.Id;

            return new DeleteContactInfoResponseModel { DeletedId = deletedId };
        }
    }
}
