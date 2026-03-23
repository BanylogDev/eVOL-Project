using Moq;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.FeaturesCases.Admin.Commands.AdminDeleteUser;
using eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser;


namespace eVOL.ApplicationTests.UseCases.AdminCases
{
    public class AdminDeleteUserTest
    {
        [Fact]
        public async Task AdminDeleteUser_DeleteUser_ReturnsUser()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<AdminDeleteUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);


            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000")
            };

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).ReturnsAsync(fakeUser);

            userRepoMock.Setup(u => u.RemoveUser(fakeUser));

            var sut = new AdminDeleteUserHandler(uowMock.Object, loggerMock.Object);


            // Act

            var result = await sut.Handle(new AdminDeleteUserCommand(Guid.Parse("00000000-0000-0000-0000-000000000000")), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeUser.UserId, result.UserId);

            userRepoMock.Verify(r => r.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Times.Once);
            userRepoMock.Verify(r => r.RemoveUser(fakeUser), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task AdminDeleteUser_UserDoesNotExist_ReturnsNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherRepoMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<AdminDeleteUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).ReturnsAsync((User?)null);

            var sut = new AdminDeleteUserHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new AdminDeleteUserCommand(Guid.Parse("00000000-0000-0000-0000-000000000000")), CancellationToken.None);

            // Assert

            Assert.Null(result);
            userRepoMock.Verify(u => u.RemoveUser(It.IsAny<User>()), Times.Never);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task AdminDeleteUser_ThrowsException_PerformsRollback()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<AdminDeleteUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(r => r.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000000")))
                        .ThrowsAsync(new Exception("DB error"));

            var sut = new AdminDeleteUserHandler(uowMock.Object, loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new AdminDeleteUserCommand(Guid.Parse("00000000-0000-0000-0000-000000000000")), CancellationToken.None));

            uowMock.Verify(u => u.RollbackAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
