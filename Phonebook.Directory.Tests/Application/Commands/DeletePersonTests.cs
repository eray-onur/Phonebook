using MediatR;

using Microsoft.EntityFrameworkCore;

using Moq;

using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Application.Models.Requests;
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
    public class DeletePersonTests
    {
        private Mock<PhonebookDbContext> mockDirectoryContext;
        private Mock<IRequestHandler<DeletePersonCommand, DeletePersonResponseModel>> handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<PhonebookDbContext>()
                .UseInMemoryDatabase(databaseName: "PhonebookTestDb")
                .Options;

            mockDirectoryContext = new Mock<PhonebookDbContext>(options);

            handler = new Mock<IRequestHandler<DeletePersonCommand, DeletePersonResponseModel>>();
        }
        [Test]
        public async Task Handle_GivenValidRequest_ShouldRemovePerson()
        {
            // Arrange
            Guid deletedPersonId = Guid.NewGuid();
            var command = new DeletePersonCommand(
                new DeletePersonRequestModel
                {
                    Id = deletedPersonId
                }
            );
            var mockResult = new DeletePersonResponseModel { DeletedId = deletedPersonId };

            handler.Setup(m => m.Handle(command, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockResult));

            // Act
            var result = await handler.Object.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(mockResult));

        }

        [Test]
        public async Task Handle_GivenInvalidId_ShouldThrowSearchFailure()
        {
            // Arrange
            Guid deletedPersonId = Guid.NewGuid();
            var command = new DeletePersonCommand(
                new DeletePersonRequestModel
                {
                    Id = deletedPersonId,
                }
            );

            var mockResult = new DeletePersonResponseModel { DeletedId = deletedPersonId };

            handler.Setup(m => m.Handle(command, It.IsAny<CancellationToken>()))
                .Throws(new Exception(ErrorMessages.FailedToFindPerson));


            string errorMsg = string.Empty;

            // Act
            try
            {
                await handler.Object.Handle(command, CancellationToken.None);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            // Assert
            Assert.That(errorMsg, Is.EqualTo(ErrorMessages.FailedToFindPerson));

        }
    }
}
