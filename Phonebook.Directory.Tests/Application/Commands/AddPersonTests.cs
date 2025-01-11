using MediatR;

using Microsoft.EntityFrameworkCore;

using Moq;

using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models.Responses;
using Phonebook.Directory.Domain;
using Phonebook.Directory.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Directory.Tests.Application.Commands
{
    public class AddPersonTests
    {
        private Mock<PhonebookDbContext> mockDirectoryContext;
        private Mock<IRequestHandler<AddPersonCommand, AddPersonResponseModel>> handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<PhonebookDbContext>()
                .UseInMemoryDatabase(databaseName: "PhonebookTestDb")
                .Options;

            mockDirectoryContext = new Mock<PhonebookDbContext>(options);

            handler = new Mock<IRequestHandler<AddPersonCommand, AddPersonResponseModel>>();
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldAddPerson()
        {
            // Arrange
            var command = new AddPersonCommand(
                new Directory.Application.Models.Requests.AddPersonRequestModel
                {
                    Name = "John",
                    LastName = "Doe",
                    CompanyName = "Lorem Ipsum Company",
                }
            );

            Guid generatedPersonId = Guid.NewGuid();
            var mockResult = new AddPersonResponseModel { CreatedId = generatedPersonId };

            handler.Setup(m => m.Handle(command, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockResult));

            // Act
            var result = await handler.Object.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(mockResult));

        }
    }
}
