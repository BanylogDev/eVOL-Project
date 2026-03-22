using eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage;
using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace eVOL.ApplicationTests.UseCases.SupportTicketCases
{
    public class SendSupportTicketMessageTest
    {

        [Fact]
        public async Task SendSupportTicketMessage_SendMessageSuccessfully_ReturnChatMessageAndUser()
        {
            // Arrange

            var uowMysqlMock = new Mock<IPostgreUnitOfWork>();
            var uowMongoMock = new Mock<IMongoUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var notificationRepoMock = new Mock<IPublisher>();
            var loggerMock = new Mock<ILogger<SendSupportTicketMessageHandler>>();

            uowMysqlMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);
            uowMysqlMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMysqlMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = 1, 
            };

            var fakeSupportTicket = new SupportTicket
            {
                Id = 1,
                Name = "Test Support Ticket"
            };

            var Message = new ChatMessage
            {
                Text = "Test",
                SenderId = 1,
                ReceiverId = 1,
                CreatedAt = DateTime.UtcNow,
                MessageId = 1,
            };

            supportTicketRepoMock.Setup(s => s.GetSupportTicketByName("Test Support Ticket"))
                .ReturnsAsync(fakeSupportTicket);

            userRepoMock.Setup(u => u.GetUserById(1))
                .ReturnsAsync(fakeUser);

            notificationRepoMock
                .Setup(n => n.Publish(It.IsAny<SendSupportTicketMessageEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);



            var sut = new SendSupportTicketMessageHandler(notificationRepoMock.Object, uowMysqlMock.Object, loggerMock.Object);

            // Act

            var (chatMessage, user) = await sut.Handle(new SendSupportTicketMessageCommand("Test Message", fakeSupportTicket.Name, fakeUser.UserId), CancellationToken.None);

            // Assert

            Assert.NotNull(chatMessage);
            Assert.NotNull(user);
            Assert.Equal(fakeUser.UserId, user.UserId);
            Assert.Equal(fakeUser.Name, user.Name);

            uowMysqlMock.Verify(u => u.BeginTransactionAsync(), Times.Once);

            uowMysqlMock.Verify(u => u.CommitAsync(), Times.Once);

            uowMysqlMock.Verify(u => u.RollbackAsync(), Times.Never);

            supportTicketRepoMock.Verify(s => s.GetSupportTicketByName(It.IsAny<string>()), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(1), Times.Once);

            notificationRepoMock.Verify(u => u.Publish(It.IsAny<SendSupportTicketMessageEvent>()), Times.Once);

        }

        [Fact]
        public async Task SendSupportTicketMessage_SupportTicketOrUserNull_ReturnNull()
        {

            // Arrange

            var uowMysqlMock = new Mock<IPostgreUnitOfWork>();
            var uowMongoMock = new Mock<IMongoUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var notificationRepoMock = new Mock<IPublisher>();
            var rabbitMqMock = new Mock<RabbitMqPublisher>();
            var loggerMock = new Mock<ILogger<SendSupportTicketMessageHandler>>();

            uowMysqlMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);
            uowMysqlMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMysqlMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            supportTicketRepoMock.Setup(s => s.GetSupportTicketByName("Test Support Ticket"))
                .ReturnsAsync((SupportTicket?)null);

            userRepoMock.Setup(u => u.GetUserById(1))
                .ReturnsAsync((User?)null);

            var sut = new SendSupportTicketMessageHandler(notificationRepoMock.Object, uowMysqlMock.Object, loggerMock.Object);

            // Act

            var (chatMessage, user) = await sut.Handle(new SendSupportTicketMessageCommand("Test Message", "Test Support Ticket", 1), CancellationToken.None);

            // Assert

            Assert.Null(chatMessage);
            Assert.Null(user);

            uowMysqlMock.Verify(u => u.BeginTransactionAsync(), Times.Once);

            uowMysqlMock.Verify(u => u.CommitAsync(), Times.Never);

            uowMysqlMock.Verify(u => u.RollbackAsync(), Times.Never);

            supportTicketRepoMock.Verify(s => s.GetSupportTicketByName(It.IsAny<string>()), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(1), Times.Once);
        }

        [Fact]
        public async Task SendSupportTicketMessage_ThrowException_ReturnNothing()
        {
            // Arrange

            var uowMysqlMock = new Mock<IPostgreUnitOfWork>();
            var uowMongoMock = new Mock<IMongoUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var notificationRepoMock = new Mock<IPublisher>();
            var rabbitMqMock = new Mock<RabbitMqPublisher>();
            var loggerMock = new Mock<ILogger<SendSupportTicketMessageHandler>>();

            uowMysqlMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);
            uowMysqlMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMysqlMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            uowMysqlMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            supportTicketRepoMock.Setup(s => s.GetSupportTicketByName("Test Support Ticket"))
                .ThrowsAsync(new Exception("Database error"));


            var sut = new SendSupportTicketMessageHandler(notificationRepoMock.Object, uowMysqlMock.Object, loggerMock.Object);

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new SendSupportTicketMessageCommand("Test Message", "Test Support Ticket", 1), CancellationToken.None));

            uowMysqlMock.Verify(u => u.BeginTransactionAsync(), Times.Once);

            uowMysqlMock.Verify(u => u.CommitAsync(), Times.Never);

            uowMysqlMock.Verify(u => u.RollbackAsync(), Times.Once);

            supportTicketRepoMock.Verify(s => s.GetSupportTicketByName(It.IsAny<string>()), Times.Once);
        }

    }
}
