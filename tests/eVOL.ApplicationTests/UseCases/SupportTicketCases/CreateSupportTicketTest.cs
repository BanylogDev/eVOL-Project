using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.SupportTicketCases.Commands.CreateSupportTicket;


namespace eVOL.ApplicationTests.UseCases.SupportTicketCases
{
    public class CreateSupportTicketTest
    {

        [Fact]
        public async Task CreateSupportTicket_CreateSupportTicketSuccessfully_ReturnSupportTicket()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<CreateSupportTicketHandler>>();

            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            uowMock.Setup(u => u.Users.GetUserById(It.IsAny<Guid>())).ReturnsAsync(fakeUser);

            supportTicketRepoMock.Setup(r => r.CreateSupportTicket(It.IsAny<SupportTicket>()));

            var sut = new CreateSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new CreateSupportTicketCommand(new SupportTicketDTO
            {
                Category = "Technical",
                Text = "I need help with my account.",
                OpenedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
            }), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            
            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            supportTicketRepoMock.Verify(r => r.CreateSupportTicket(It.IsAny<SupportTicket>()), Times.Once);
        }

        [Fact]
        public async Task CreateSupportTicket_ThrowException_ReturnNothing()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var loggerMock = new Mock<ILogger<CreateSupportTicketHandler>>();


            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            uowMock.Setup(u => u.Users.GetUserById(It.IsAny<Guid>())).ThrowsAsync(new Exception("Database error"));

            var sut = new CreateSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act & Assert 

            await Assert.ThrowsAsync<Exception>(async () =>
                await sut.Handle(new CreateSupportTicketCommand(new SupportTicketDTO
                {
                    Category = "Technical",
                    Text = "I need help with my account.",
                    OpenedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
                }), CancellationToken.None)
            );

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            uowMock.Verify(u => u.Users.GetUserById(It.IsAny<Guid>()), Times.Once);

        }

    }
}
