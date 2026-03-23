using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket;


namespace eVOL.ApplicationTests.UseCases.SupportTicketCases
{
    public class UnClaimSupportTicketTest
    {

        [Fact]
        public async Task UnClaimSupportTicket_UnClaimSupportTicketSuccessfully_ReturnUser()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<UnClaimSupportTicketHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            var fakeSupportTicket = new SupportTicket
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ClaimedStatus = true,
                ClaimedById = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                OpenedById = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ClaimedBy = fakeUser,
            };

            userRepoMock.Setup(r => r.GetUserById(It.IsAny<Guid>())).ReturnsAsync(fakeUser);

            supportTicketRepoMock.Setup(r => r.GetSupportTicketById(It.IsAny<Guid>())).ReturnsAsync(fakeSupportTicket);

            var sut = new UnClaimSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new UnClaimSupportTicketCommand(new ClaimSupportTicketDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                OpenedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
            }),CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeUser.UserId, result.UserId);
            Assert.False(fakeSupportTicket.ClaimedStatus);
            Assert.NotEqual(fakeUser.UserId, fakeSupportTicket.ClaimedById);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(r => r.GetUserById(It.IsAny<Guid>()), Times.Once);

            supportTicketRepoMock.Verify(r => r.GetSupportTicketById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UnClaimSupportTicket_UserOrSupportTicketNull_ReturnNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<UnClaimSupportTicketHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(r => r.GetUserById(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            supportTicketRepoMock.Setup(r => r.GetSupportTicketById(It.IsAny<Guid>())).ReturnsAsync((SupportTicket?)null);

            var sut = new UnClaimSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new UnClaimSupportTicketCommand(new ClaimSupportTicketDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                OpenedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
            }), CancellationToken.None);

            // Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(r => r.GetUserById(It.IsAny<Guid>()), Times.Once);

            supportTicketRepoMock.Verify(r => r.GetSupportTicketById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UnClaimSupportTicket_AlreadyUnClaimed_ReturnNull()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<UnClaimSupportTicketHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            var fakeSupportTicket = new SupportTicket
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ClaimedStatus = false,
                ClaimedById = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                OpenedById = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                OpenedBy = fakeUser,
            };

            userRepoMock.Setup(r => r.GetUserById(It.IsAny<Guid>())).ReturnsAsync(fakeUser);

            supportTicketRepoMock.Setup(r => r.GetSupportTicketById(It.IsAny<Guid>())).ReturnsAsync(fakeSupportTicket);

            var sut = new UnClaimSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new UnClaimSupportTicketCommand(new ClaimSupportTicketDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                OpenedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
            }), CancellationToken.None);

            // Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(r => r.GetUserById(It.IsAny<Guid>()), Times.Once);

            supportTicketRepoMock.Verify(r => r.GetSupportTicketById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UnClaimSupportTicket_ThrowException_ReturnNothing()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<UnClaimSupportTicketHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(r => r.GetUserById(It.IsAny<Guid>())).ThrowsAsync(new Exception("Database error"));

            var sut = new UnClaimSupportTicketHandler(uowMock.Object, loggerMock.Object);

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new UnClaimSupportTicketCommand(new ClaimSupportTicketDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                OpenedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
            }), CancellationToken.None));

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            userRepoMock.Verify(r => r.GetUserById(It.IsAny<Guid>()), Times.Once);

            supportTicketRepoMock.Verify(r => r.GetSupportTicketById(It.IsAny<Guid>()), Times.Never);
        }


    }
}
