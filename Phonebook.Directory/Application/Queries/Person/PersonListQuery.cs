using MediatR;

using Microsoft.EntityFrameworkCore;

using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models.Responses.Person;
using Phonebook.Directory.Domain;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Application.Queries.Person
{
    public record PersonListQuery(): IRequest<PersonListResponseModel>;

    public class PersonListQueryHandler : IRequestHandler<PersonListQuery, PersonListResponseModel>
    {
        private readonly PhonebookDbContext phonebookContext;

        public PersonListQueryHandler(PhonebookDbContext phonebookContext)
        {
            this.phonebookContext = phonebookContext;
        }

        public async Task<PersonListResponseModel> Handle(PersonListQuery request, CancellationToken cancellationToken)
        {
            var personDbList = await phonebookContext.Set<Domain.Person>().ToListAsync(cancellationToken);

            var result = new PersonListResponseModel();

            foreach(var personDb in personDbList)
            {
                var newPerson = new PersonListItem
                {
                    Id = personDb.Id,
                    Name = personDb.Name,
                    LastName = personDb.LastName,
                    CompanyName = personDb.CompanyName,
                };
                result.PersonList.Add(newPerson);
            }

            return result;
        }
    }
}
