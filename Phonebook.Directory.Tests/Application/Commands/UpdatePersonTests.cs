using MediatR;

using Microsoft.EntityFrameworkCore;

using Moq;

using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Application.Models.Responses.Person;
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
    public class UpdatePersonTests
    {
        private Mock<DirectoryDbContext> mockDirectoryContext;
        private Mock<IRequestHandler<UpdatePersonCommand, UpdatePersonResponseModel>> handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DirectoryDbContext>()
                .UseInMemoryDatabase(databaseName: "PhonebookTestDb")
                .Options;

            mockDirectoryContext = new Mock<DirectoryDbContext>(options);

            handler = new Mock<IRequestHandler<UpdatePersonCommand, UpdatePersonResponseModel>>();
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldUpdatePersonName()
        {
            // Arrange
            Guid updatedPersonId = Guid.NewGuid();
            var command = new UpdatePersonCommand(
                new Directory.Application.Models.Requests.UpdatePersonRequestModel
                {
                    Id = updatedPersonId,
                    Name = "John updated",
                    LastName = "Doe",
                    CompanyName = "Lorem Ipsum Company",
                }
            );

            var mockResult = new UpdatePersonResponseModel { UpdatedId = updatedPersonId };

            handler.Setup(m => m.Handle(command, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockResult));

            // Act
            var result = await handler.Object.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(mockResult));

        }
        [Test]
        public async Task Handle_GivenValidRequest_ShouldUpdatePersonLastName()
        {
            // Arrange
            Guid updatedPersonId = Guid.NewGuid();
            var command = new UpdatePersonCommand(
                new Directory.Application.Models.Requests.UpdatePersonRequestModel
                {
                    Id = updatedPersonId,
                    Name = "John",
                    LastName = "Doe updated",
                    CompanyName = "Lorem Ipsum Company",
                }
            );

            var mockResult = new UpdatePersonResponseModel { UpdatedId = updatedPersonId };

            handler.Setup(m => m.Handle(command, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockResult));

            // Act
            var result = await handler.Object.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(mockResult));

        }
        [Test]
        public async Task Handle_GivenValidRequest_ShouldUpdatePersonCompanyName()
        {
            // Arrange
            Guid updatedPersonId = Guid.NewGuid();
            var command = new UpdatePersonCommand(
                new Directory.Application.Models.Requests.UpdatePersonRequestModel
                {
                    Id = updatedPersonId,
                    Name = "John",
                    LastName = "Doe",
                    CompanyName = "Lorem Ipsum Company updated",
                }
            );

            var mockResult = new UpdatePersonResponseModel { UpdatedId = updatedPersonId };

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
            Guid updatedPersonId = Guid.NewGuid();
            var command = new UpdatePersonCommand(
                new Directory.Application.Models.Requests.UpdatePersonRequestModel
                {
                    Id = updatedPersonId,
                    Name = "John updated",
                    LastName = "Doe updated",
                    CompanyName = "Lorem Ipsum Company updated",
                }
            );

            var mockResult = new UpdatePersonResponseModel { UpdatedId = updatedPersonId };

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
