using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.Features.ChatGroupCases.Commands.RemoveUserFromChatGroup;

namespace eVOL.ApplicationTests.UseCases.ChatGroupCases
{
    public class RemoveUserFromChatGroupTest
    {
        [Fact]
        public async Task RemoveUserFromChatGroup_RemoveUserFromChatGroup_ReturnsUser()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<RemoveUserFromChatGroupHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            };

            var fakeChatGroup = new ChatGroup
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "TestGroup",
                TotalUsers = 2,
                GroupUsers = new List<User> { fakeUser },
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            userRepoMock.Setup(u => u.GetUserById(It.IsAny<Guid>())).ReturnsAsync(fakeUser);

            chatGroupRepoMock.Setup(c => c.GetChatGroupByName(It.IsAny<string>())).ReturnsAsync(fakeChatGroup);

            var sut = new RemoveUserFromChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new RemoveUserFromChatGroupCommand(fakeUser.UserId, "TestGroup"), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeUser.UserId, result.UserId);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(fakeUser.UserId), Times.Once);

            chatGroupRepoMock.Verify(c => c.GetChatGroupByName("TestGroup"), Times.Once);
        }

        [Fact]
        public async Task RemoveUserFromChatGroup_UserNotInTheChatGroup_ReturnsNull()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<RemoveUserFromChatGroupHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            var fakeChatGroup = new ChatGroup
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "TestGroup",
                TotalUsers = 1,
                GroupUsers = new List<User>(),
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            };
            userRepoMock.Setup(u => u.GetUserById(It.IsAny<Guid>())).ReturnsAsync(fakeUser);

            chatGroupRepoMock.Setup(c => c.GetChatGroupByName(It.IsAny<string>())).ReturnsAsync(fakeChatGroup);

            var sut = new RemoveUserFromChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new RemoveUserFromChatGroupCommand(fakeUser.UserId, "TestGroup"), CancellationToken.None);

            // Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(fakeUser.UserId), Times.Once);

            chatGroupRepoMock.Verify(c => c.GetChatGroupByName("TestGroup"), Times.Once);
        }

        [Fact]
        public async Task RemoveUserFromChatGroup_UserIsOwner_ReturnNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<RemoveUserFromChatGroupHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            };

            var fakeChatGroup = new ChatGroup
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "TestGroup",
                TotalUsers = 1,
                GroupUsers = new List<User> { fakeUser },
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            };

            userRepoMock.Setup(u => u.GetUserById(It.IsAny<Guid>())).ReturnsAsync(fakeUser);

            chatGroupRepoMock.Setup(c => c.GetChatGroupByName(It.IsAny<string>())).ReturnsAsync(fakeChatGroup);

            var sut = new RemoveUserFromChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new RemoveUserFromChatGroupCommand(fakeUser.UserId, "TestGroup"), CancellationToken.None);

            // Assert

            Assert.Null(result);
            Assert.Equal(fakeChatGroup.OwnerId, fakeUser.UserId);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(fakeUser.UserId), Times.Once);

            chatGroupRepoMock.Verify(c => c.GetChatGroupByName("TestGroup"), Times.Once);
        }

        [Fact]
        public async Task RemoveUserFromChatGroup_ThrowException_ReturnNothing()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<RemoveUserFromChatGroupHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(u => u.GetUserById(It.IsAny<Guid>())).ThrowsAsync(new Exception("Database error"));

            var sut = new RemoveUserFromChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await sut.Handle(new RemoveUserFromChatGroupCommand(Guid.Parse("00000000-0000-0000-0000-000000000001"), "TestGroup"), CancellationToken.None);
            });

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
