using Moq;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.UserCases.Commands.DeleteUser;


namespace eVOL.ApplicationTests.UseCases.UserCases
{
    public class DeleteUserTest
    {
        [Fact]
        public async Task DeleteUser_DeleteUser_ReturnsUser()
        {
            //Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<DeleteUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "hashedPassword"
            };

            var fakeDeleteAccountDTO = new DeleteAccountDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password",
                ConfirmPassword = "password"
            };

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync(fakeUser);
            userRepoMock.Setup(u => u.RemoveUser(fakeUser));

            passwordHasherMock.Setup(p => p.VerifyPassword(fakeDeleteAccountDTO.Password, fakeUser.Password)).Returns(true);

            var sut = new DeleteUserHandler(uowMock.Object, passwordHasherMock.Object, loggerMock.Object);

            //Act

            var result = await sut.Handle(new DeleteUserCommand(fakeDeleteAccountDTO), CancellationToken.None);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(fakeUser.UserId, result.UserId);
            Assert.Equal(fakeDeleteAccountDTO.Password, fakeDeleteAccountDTO.ConfirmPassword);


            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);
            userRepoMock.Verify(u => u.RemoveUser(fakeUser), Times.Once);

        }

        [Fact]
        public async Task DeleteUser_UserNotFound_ReturnsNull()
        {
            //Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<DeleteUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeDeleteAccountDTO = new DeleteAccountDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password",
                ConfirmPassword = "password"
            };

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync((User?)null);

            var sut = new DeleteUserHandler(uowMock.Object, passwordHasherMock.Object, loggerMock.Object);

            //Act

            var result = await sut.Handle(new DeleteUserCommand(fakeDeleteAccountDTO), CancellationToken.None);

            //Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")   ), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ExceptionTrhown_RollbackCalled()
        {
            //Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<DeleteUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeDeleteAccountDTO = new DeleteAccountDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password",
                ConfirmPassword = "password"
            };

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ThrowsAsync(new Exception("Database error"));

            var sut = new DeleteUserHandler(uowMock.Object, passwordHasherMock.Object, loggerMock.Object);

            //Act & Assert

            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new DeleteUserCommand(fakeDeleteAccountDTO), CancellationToken.None));

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);
        }

    }
}
