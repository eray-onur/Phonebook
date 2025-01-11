using MediatR;

using Microsoft.EntityFrameworkCore;

using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Application.Models.Requests.Person;
using Phonebook.Directory.Application.Models.Responses.Person;
using Phonebook.Directory.Domain;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Application.Queries.Person
{
    public record PersonDetailsQuery(PersonDetailsRequestModel model) : IRequest<PersonDetailsResponseModel>;

    public class PersonDetailsQueryHandler : IRequestHandler<PersonDetailsQuery, PersonDetailsResponseModel>
    {
        private readonly PhonebookDbContext phonebookContext;

        public PersonDetailsQueryHandler(PhonebookDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<PersonDetailsResponseModel> Handle(PersonDetailsQuery request, CancellationToken cancellationToken)
        {
            var existingPerson = await phonebookContext.Set<Domain.Person>().FirstOrDefaultAsync(p => p.Id.Equals(request.model.Id), cancellationToken);
            if (existingPerson == null)
                throw new Exception(ErrorMessages.FailedToFindPerson);

            var existingContactInfo = await phonebookContext.Set<Domain.ContactInfo>()
                .Where(ci => ci.PersonId.Equals(existingPerson.Id))
                .ToListAsync(cancellationToken);

            var result = new PersonDetailsResponseModel
            {
                Id = existingPerson.Id,
                Name = existingPerson.Name,
                LastName = existingPerson.LastName,
                CompanyName = existingPerson.CompanyName,
            };

            foreach (var contactInfo in existingContactInfo)
            {
                var contactInfoItem = new PersonContactInfoDetailsItem
                {
                    PhoneNumber = contactInfo.PhoneNumber,
                    Email = contactInfo.Email,
                    Location = contactInfo.Location,
                    Description = contactInfo.Description,
                };
                result.ContactInfos.Add(contactInfoItem);
            }

            return result;
        }
    }
}
