using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.Features.SupportTicketCases.Commands.DeleteSupportTicket;


namespace eVOL.ApplicationTests.UseCases.SupportTicketCases
{
    public class DeleteSupportTicketTest
    {

        [Fact]
        public async Task DeleteSupportTicket_DeleteSupportTicketSuccessfully_ReturnSupportTicket()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<DeleteSupportTicketHandler>>();

            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeSupportTicket = new SupportTicket
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Category = "Test",
                Text = "Test Message",
                OpenedById = Guid.Parse("00000000-0000-0000-0000-000000000001")
            };

            supportTicketRepoMock.Setup(r => r.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync(fakeSupportTicket);
            supportTicketRepoMock.Setup(r => r.DeleteSupportTicket(fakeSupportTicket));

            var sut = new DeleteSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new DeleteSupportTicketCommand(Guid.Parse("00000000-0000-0000-0000-000000000001")), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000001"), result.Id);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            supportTicketRepoMock.Verify(r => r.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);
            supportTicketRepoMock.Verify(r => r.DeleteSupportTicket(fakeSupportTicket), Times.Once);

        }

        [Fact]
        public async Task DeleteSupportTicket_SupportTicketNull_ReturnNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<DeleteSupportTicketHandler>>();

            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            supportTicketRepoMock.Setup(r => r.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync((SupportTicket?)null);

            var sut = new DeleteSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new DeleteSupportTicketCommand(Guid.Parse("00000000-0000-0000-0000-000000000001")), CancellationToken.None);

            // Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            supportTicketRepoMock.Verify(r => r.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);
        }

        [Fact]
        public async Task DeleteSupportTicket_ThrowException_ReturnNothing()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<DeleteSupportTicketHandler>>();

            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            supportTicketRepoMock.Setup(s => s.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001")))
                .ThrowsAsync(new Exception("Database error"));

            var sut = new DeleteSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new DeleteSupportTicketCommand(Guid.Parse("00000000-0000-0000-0000-000000000001")), CancellationToken.None));

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            supportTicketRepoMock.Verify(s => s.GetSupportTicketById(It.IsAny<Guid>()), Times.Once);

        }
    }
}
