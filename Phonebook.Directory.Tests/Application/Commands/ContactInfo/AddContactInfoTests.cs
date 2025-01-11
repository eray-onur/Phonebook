using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

using Phonebook.Directory.Application.Commands.ContactInfo;
using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Application.Models.Requests.ContactInfo;
using Phonebook.Directory.Application.Models.Responses;
using Phonebook.Directory.Application.Models.Responses.ContactInfo;
using Phonebook.Directory.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Directory.Tests.Application.Commands.ContactInfo
{
    public class AddContactInfoTests
    {
        private Mock<PhonebookDbContext> mockDirectoryContext;
        private Mock<IRequestHandler<AddContactInfoCommand, AddContactInfoResponseModel>> handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<PhonebookDbContext>()
                .UseInMemoryDatabase(databaseName: "PhonebookTestDb")
                .Options;

            mockDirectoryContext = new Mock<PhonebookDbContext>(options);

            handler = new Mock<IRequestHandler<AddContactInfoCommand, AddContactInfoResponseModel>>();
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldAddPerson()
        {
            // Arrange
            var command = new AddContactInfoCommand(
                new AddContactInfoRequestModel
                {
                    PersonId = Guid.NewGuid(),
                    Email = "example@google.co",
                    PhoneNumber = "905413259711",
                    Location = "Somewhere lorem ipsum",
                    Description = "aa"
                }
            );

            Guid generatedContactInfoId = Guid.NewGuid();
            var mockResult = new AddContactInfoResponseModel { AddedContactInfoId = generatedContactInfoId };

            handler.Setup(m => m.Handle(command, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockResult));

            // Act
            var result = await handler.Object.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(mockResult));

        }

        [Test]
        public Task Handle_GivenInvalidPhone_ShouldThrow()
        {
            // Arrange
            var command = new AddContactInfoCommand(
                new AddContactInfoRequestModel
                {
                    PersonId = Guid.NewGuid(),
                    Email = "example@google.co",
                    PhoneNumber = "aeeoeeboaboa",
                    Location = "Somewhere lorem ipsum",
                    Description = "aa"
                }
            );

            Guid generatedContactInfoId = Guid.NewGuid();
            var mockResult = new AddContactInfoResponseModel { AddedContactInfoId = generatedContactInfoId };

            handler.Setup(m => m.Handle(command, It.IsAny<CancellationToken>()))
                .Throws(new CommandValidationException(ErrorMessages.InvalidPhoneNumber));

            // Act
            async Task Act() => await handler.Object.Handle(command, CancellationToken.None);

            // Assert
            var ex = Assert.ThrowsAsync<CommandValidationException>(Act);
            Assert.That(ex.Message, Is.EqualTo(ErrorMessages.InvalidPhoneNumber));
            return Task.CompletedTask;
        }
    }
}
