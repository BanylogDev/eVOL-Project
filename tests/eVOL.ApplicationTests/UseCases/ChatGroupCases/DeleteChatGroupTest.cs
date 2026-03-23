using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.Features.ChatGroupCases.Commands.DeleteChatGroup;
using eVOL.Application.DTOs;



namespace eVOL.ApplicationTests.UseCases.ChatGroupCases
{
    public class DeleteChatGroupTest
    {

        [Fact]
        public async Task DeleteChatGroup_DeleteChatGroupSuccessfully_ReturnsChatGroup()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<DeleteChatGroupHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeChatGroup = new ChatGroup
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "TestGroup",
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).ReturnsAsync(fakeChatGroup);
            chatGroupRepoMock.Setup(c => c.DeleteChatGroup(fakeChatGroup));

            var sut = new DeleteChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new DeleteChatGroupCommand(new DeleteChatGroupDTO { ChatGroupId= Guid.Parse("00000000-0000-0000-0000-000000000000"), ChatGroupOwnerId= Guid.Parse("00000000-0000-0000-0000-000000000001") }), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeChatGroup.Name, result.Name);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Times.Once);
            chatGroupRepoMock.Verify(c => c.DeleteChatGroup(fakeChatGroup), Times.Once);
        }

        [Fact]
        public async Task DeleteChatGroup_ChatGroupNull_ReturnNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<DeleteChatGroupHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).ReturnsAsync((ChatGroup?)null);

            var sut = new DeleteChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new DeleteChatGroupCommand(new DeleteChatGroupDTO { ChatGroupId = Guid.Parse("00000000-0000-0000-0000-000000000000"), ChatGroupOwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001") }), CancellationToken.None);

            // Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Times.Once);
            chatGroupRepoMock.Verify(c => c.DeleteChatGroup(It.IsAny<ChatGroup>()), Times.Never);
        }

        [Fact]
        public async Task DeleteChatGroup_ThrowsException_ReturnsNothing()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<DeleteChatGroupHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).ThrowsAsync(new Exception("Database error"));

            var sut = new DeleteChatGroupHandler(uowMock.Object, loggerMock.Object);

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new DeleteChatGroupCommand(new DeleteChatGroupDTO { ChatGroupId = Guid.Parse("00000000-0000-0000-0000-000000000000"), ChatGroupOwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001") }), CancellationToken.None));

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Times.Once);
        }

    }
}
