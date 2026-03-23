using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.Features.ChatGroupCases.Commands.TransferOwnershipOfChatGroup;
using eVOL.Application.DTOs.Requests;


namespace eVOL.ApplicationTests.UseCases.ChatGroupCases
{
    public class TransferOwnershipOfChatGroupTest
    {

        [Fact]
        public async Task TransferOwnershipOfChatGroup_SuccessfullyTransferOwnership_ReturnChatGroup()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<TransferOwnershipOfChatGroupHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);
            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeCurrentOwner = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "CurrentOwner"
            };

            var fakeNewOwner = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "NewOwner"
            };

            var fakeChatGroup = new ChatGroup
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "TestGroup",
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            };

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")))
                .ReturnsAsync(fakeCurrentOwner);

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000002")))
                .ReturnsAsync(fakeNewOwner);

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000001")))
                .ReturnsAsync(fakeChatGroup);

            var sut = new TransferOwnershipOfChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new TransferOwnershipOfChatGroupCommand(
                new TransferOwnershipOfCGDTO { CurrentOwnerId=fakeCurrentOwner.UserId, NewOwnerId=fakeNewOwner.UserId, ChatGroupId=fakeChatGroup.Id }), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeNewOwner.UserId, result.OwnerId);
            Assert.Equal(fakeChatGroup.Id, result.Id);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(It.IsAny<Guid>()), Times.Exactly(2));

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(It.IsAny<Guid>()), Times.Once);

        }

        [Fact]
        public async Task TransferOwnershipOfChatGroup_NewOwnerOrCurrentOwnerOrChatGroupNull_ReturnNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<TransferOwnershipOfChatGroupHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);
            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(u => u.GetUserById(It.IsAny<Guid>()))
                .ReturnsAsync((User?)null);

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(It.IsAny<Guid>()))
                .ReturnsAsync((ChatGroup?)null);

            var sut = new TransferOwnershipOfChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new TransferOwnershipOfChatGroupCommand(
                new TransferOwnershipOfCGDTO { CurrentOwnerId = Guid.Parse("00000000-0000-0000-0000-000000000000"), NewOwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), ChatGroupId = Guid.Parse("00000000-0000-0000-0000-000000000000") }), CancellationToken.None);

            // Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(It.IsAny<Guid>()), Times.Exactly(2));

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task TransferOwnershipOfChatGroup_CurrentOwnerNotOwner_ReturnNull()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<TransferOwnershipOfChatGroupHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);
            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeCurrentOwner = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "CurrentOwner"
            };

            var fakeNewOwner = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "NewOwner"
            };

            var fakeChatGroup = new ChatGroup
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "TestGroup",
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            };

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000000")))
                .ReturnsAsync(fakeCurrentOwner);

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")))
                .ReturnsAsync(fakeNewOwner);

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(It.IsAny<Guid>()))
                .ReturnsAsync(fakeChatGroup);

            var sut = new TransferOwnershipOfChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new TransferOwnershipOfChatGroupCommand(
                new TransferOwnershipOfCGDTO { CurrentOwnerId = fakeCurrentOwner.UserId, NewOwnerId = fakeNewOwner.UserId, ChatGroupId = fakeChatGroup.Id }), CancellationToken.None);

            // Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(It.IsAny<Guid>()), Times.Exactly(2));

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task TransferOwnershipOfChatGroup_ExceptionThrown_RollbackTransaction()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<TransferOwnershipOfChatGroupHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);
            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(u => u.GetUserById(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Database error"));

            var sut = new TransferOwnershipOfChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new TransferOwnershipOfChatGroupCommand(
                new TransferOwnershipOfCGDTO { CurrentOwnerId = Guid.Parse("00000000-0000-0000-0000-000000000000"), NewOwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"), ChatGroupId = Guid.Parse("00000000-0000-0000-0000-000000000000") }), CancellationToken.None));

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(It.IsAny<Guid>()), Times.Once);

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(It.IsAny<Guid>()), Times.Never);
        }


    }
}
