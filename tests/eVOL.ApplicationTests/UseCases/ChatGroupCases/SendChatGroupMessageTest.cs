using eVOL.Application.Features.ChatGroupCases.Commands.SendChatGroupMessage;
using eVOL.Application.Messaging.Interfaces;
using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;


namespace eVOL.ApplicationTests.UseCases.ChatGroupCases
{
    public class SendChatGroupMessageTest
    {
        [Fact]
        public async Task SendChatGroupMessage_SendMessageToChatGroup_ReturnsChatMessageAndUser()
        {
            // Arrange

            var uowMysqlMock = new Mock<IPostgreUnitOfWork>();
            var uowMongoMock = new Mock<IMongoUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var notificationRepoMock = new Mock<IPublisher>();
            var loggerMock = new Mock<ILogger<SendChatGroupMessageHandler>>();

            uowMysqlMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);
            uowMysqlMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMysqlMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "User"
            };

            var fakeChatGroup = new ChatGroup
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "TestGroup"
            };

            var Message = new ChatMessage
            {
                Text = "Test",
                SenderId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                ReceiverId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                CreatedAt = DateTime.UtcNow,
                MessageId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            };

            userRepoMock.Setup(u => u.GetUserById(It.IsAny<Guid>()))
                .ReturnsAsync(fakeUser);

            chatGroupRepoMock.Setup(c => c.GetChatGroupByName(It.IsAny<string>()))
                .ReturnsAsync(fakeChatGroup);

            notificationRepoMock
                .Setup(n => n.Publish(It.IsAny<SendChatGroupMessageEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);


            var sut = new SendChatGroupMessageHandler(notificationRepoMock.Object, uowMysqlMock.Object, loggerMock.Object);

            // Act

            var (chatMessage, user) = await sut.Handle(new SendChatGroupMessageCommand("Test Message", "TestGroup", fakeUser.UserId), CancellationToken.None);

            // Assert

            Assert.NotNull(chatMessage);
            Assert.NotNull(user);
            Assert.Equal("Test Message", chatMessage.Text);
            Assert.Equal(fakeUser.UserId, chatMessage.SenderId);
            Assert.Equal(fakeChatGroup.Id, chatMessage.ReceiverId);

            uowMysqlMock.Verify(u => u.BeginTransactionAsync(), Times.Once);

            uowMysqlMock.Verify(u => u.CommitAsync(), Times.Once);

            uowMysqlMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(It.IsAny<Guid>()), Times.Once);

            chatGroupRepoMock.Verify(c => c.GetChatGroupByName(It.IsAny<string>()), Times.Once);

            notificationRepoMock.Verify(c => c.Publish(It.IsAny<SendChatGroupMessageEvent>()), Times.Once);

        }

        [Fact]
        public async Task SendChatGroupMessage_ChatGroupOrUserNull_ReturnNull()
        {
            // Arrange

            var uowMysqlMock = new Mock<IPostgreUnitOfWork>();
            var uowMongoMock = new Mock<IMongoUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var notificationRepoMock = new Mock<IPublisher>();
            var rabbitMqMock = new Mock<IRabbitMqPublisher>();
            var loggerMock = new Mock<ILogger<SendChatGroupMessageHandler>>();

            uowMysqlMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);
            uowMysqlMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMysqlMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "User"
            };

            userRepoMock.Setup(u => u.GetUserById(It.IsAny<Guid>()))
                .ReturnsAsync((User?)null);

            chatGroupRepoMock.Setup(c => c.GetChatGroupByName(It.IsAny<string>()))
                .ReturnsAsync((ChatGroup?)null);

            var sut = new SendChatGroupMessageHandler(notificationRepoMock.Object, uowMysqlMock.Object, loggerMock.Object);

            // Act

            var (chatMessage, user) = await sut.Handle(new SendChatGroupMessageCommand("Test Message", "TestGroup", fakeUser.UserId), CancellationToken.None);

            // Assert

            Assert.Null(chatMessage);
            Assert.Null(user);

            uowMysqlMock.Verify(u => u.BeginTransactionAsync(), Times.Once);

            uowMysqlMock.Verify(u => u.CommitAsync(), Times.Never);

            uowMysqlMock.Verify(u => u.RollbackAsync(), Times.Never);

            chatGroupRepoMock.Verify(c => c.GetChatGroupByName(It.IsAny<string>()), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(It.IsAny<Guid>()), Times.Once);

            notificationRepoMock.Verify(c => c.Publish(It.IsAny<ChatMessage>()), Times.Never);

            rabbitMqMock.Verify(r => r.PublishAsync(It.IsAny<ChatMessage>()), Times.Never);
        }

        [Fact]
        public async Task SendChatGroupMessage_ThrowException_ReturnNothing()
        {
            // Arrange
            var uowMysqlMock = new Mock<IPostgreUnitOfWork>();
            var uowMongoMock = new Mock<IMongoUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var notificationRepoMock = new Mock<IPublisher>();
            var rabbitMqMock = new Mock<IRabbitMqPublisher>();
            var loggerMock = new Mock<ILogger<SendChatGroupMessageHandler>>();

            uowMysqlMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);
            uowMysqlMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMysqlMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            chatGroupRepoMock.Setup(c => c.GetChatGroupByName(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));

            var sut = new SendChatGroupMessageHandler(notificationRepoMock.Object, uowMysqlMock.Object, loggerMock.Object);

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new SendChatGroupMessageCommand("Test Message", "TestGroup", Guid.Parse("00000000-0000-0000-0000-000000000000")), CancellationToken.None));

            uowMysqlMock.Verify(u => u.BeginTransactionAsync(), Times.Once);

            uowMysqlMock.Verify(u => u.CommitAsync(), Times.Never);

            uowMysqlMock.Verify(u => u.RollbackAsync(), Times.Once);

            chatGroupRepoMock.Verify(c => c.GetChatGroupByName(It.IsAny<string>()), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(It.IsAny<Guid>()), Times.Never);

            notificationRepoMock.Verify(c => c.Publish(It.IsAny<ChatMessage>()), Times.Never);

            rabbitMqMock.Verify(r => r.PublishAsync(It.IsAny<ChatMessage>()), Times.Never);
        }
    }
}
