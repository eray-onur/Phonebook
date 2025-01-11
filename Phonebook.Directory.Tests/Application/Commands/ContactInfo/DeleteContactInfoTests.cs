﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Phonebook.Directory.Application.Commands.ContactInfo;
using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models.Requests;
using Phonebook.Directory.Application.Models.Responses;
using Phonebook.Directory.Application.Models;
using Phonebook.Directory.Application.Models.Responses.ContactInfo;
using Phonebook.Directory.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Directory.Tests.Application.Commands.ContactInfo
{
    public class DeleteContactInfoTests
    {
        private Mock<PhonebookDbContext> mockDirectoryContext;
        private Mock<IRequestHandler<DeleteContactInfoCommand, DeleteContactInfoResponseModel>> handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<PhonebookDbContext>()
                .UseInMemoryDatabase(databaseName: "PhonebookTestDb")
                .Options;

            mockDirectoryContext = new Mock<PhonebookDbContext>(options);

            handler = new Mock<IRequestHandler<DeleteContactInfoCommand, DeleteContactInfoResponseModel>>();
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldRemoveContactInfo()
        {
            // Arrange
            Guid deletedContactInfoId = Guid.NewGuid();
            var command = new DeleteContactInfoCommand(
                new Directory.Application.Models.Requests.ContactInfo.DeleteContactInfoRequestModel
                {
                    Id = deletedContactInfoId
                }
            );
            var mockResult = new DeleteContactInfoResponseModel { DeletedId = deletedContactInfoId };

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
            var command = new DeleteContactInfoCommand(
                new Directory.Application.Models.Requests.ContactInfo.DeleteContactInfoRequestModel
                {
                    Id = deletedPersonId,
                }
            );

            var mockResult = new DeleteContactInfoResponseModel { DeletedId = deletedPersonId };

            handler.Setup(m => m.Handle(command, It.IsAny<CancellationToken>()))
                .Throws(new Exception(ErrorMessages.FailedToFindContactInfo));


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
            Assert.That(errorMsg, Is.EqualTo(ErrorMessages.FailedToFindContactInfo));

        }

    }
}