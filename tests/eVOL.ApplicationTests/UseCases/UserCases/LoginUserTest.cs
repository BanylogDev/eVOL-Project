using Moq;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.DTOs;
using eVOL.Application.Features.UserCases.Commands.LoginUser;
using Microsoft.Extensions.Options;
using eVOL.Application.Options;


namespace eVOL.ApplicationTests.UseCases.UserCases
{
    public class LoginUserTest
    {
        [Fact]
        public async Task LoginUser_LoginUserWithCredentials_ReturnsMappedUser()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var jwtServiceMock = new Mock<IJwtService>();
            var optionsMock = new Mock<IOptions<JwtOptions>>();
            var loggerMock = new Mock<ILogger<LoginUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Email = "email",
                Name = "username",
                Password = "hashedPassword"
            };

            uowMock.Setup(u => u.Users.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(fakeUser);

            passwordHasherMock.Setup(p => p.VerifyPassword("password", "hashedPassword")).Returns(true);

            jwtServiceMock.Setup(j => j.GenerateJwtToken(fakeUser, It.IsAny<IOptions<JwtOptions>>())).Returns("accessToken");
            jwtServiceMock.Setup(j => j.GenerateRefreshToken()).Returns("refresh");

            var sut = new LoginUserHandler(
                uowMock.Object,
                passwordHasherMock.Object,
                jwtServiceMock.Object,
                optionsMock.Object,
                loggerMock.Object
            );

            // Act

            var result = await sut.Handle(new LoginUserCommand(new LoginDTO
            {
                Email = "email",
                Password = "password"
            }),CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeUser.UserId, result.UserId);
            Assert.Equal("email", result.Email);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            uowMock.Verify(u => u.Users.GetUserByEmail(It.IsAny<string>()), Times.Once);

            passwordHasherMock.Verify(p => p.VerifyPassword("password", "hashedPassword"), Times.Once);

            jwtServiceMock.Verify(j => j.GenerateJwtToken(fakeUser, It.IsAny<IOptions<JwtOptions>>()), Times.Once);
            jwtServiceMock.Verify(j => j.GenerateRefreshToken(), Times.Once);

        }

        [Fact] 
        public async Task LoginUser_LoginUserNull_ReturnsNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var jwtServiceMock = new Mock<IJwtService>();
            var optionsMock = new Mock<IOptions<JwtOptions>>();
            var loggerMock = new Mock<ILogger<LoginUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

            uowMock.Setup(u => u.Users.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((User?)null);

            var sut = new LoginUserHandler(
                uowMock.Object,
                passwordHasherMock.Object,
                jwtServiceMock.Object,
                optionsMock.Object,
                loggerMock.Object
            );

            // Act

            var result = await sut.Handle(new LoginUserCommand(new LoginDTO
            {
                Email = "email",
                Password = "password"
            }), CancellationToken.None);

            // Assert

            Assert.Null(result);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);

            uowMock.Verify(u => u.Users.GetUserByEmail(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task LoginUser_ThrowException_ReturnsException()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var jwtServiceMock = new Mock<IJwtService>();
            var optionsMock = new Mock<IOptions<JwtOptions>>();
            var loggerMock = new Mock<ILogger<LoginUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            uowMock.Setup(u => u.Users.GetUserByEmail(It.IsAny<string>())).ThrowsAsync(new Exception("Database error"));

            var sut = new LoginUserHandler(
                uowMock.Object,
                passwordHasherMock.Object,
                jwtServiceMock.Object,
                optionsMock.Object,
                loggerMock.Object
            );

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await sut.Handle(new LoginUserCommand(new LoginDTO
                {
                    Email = "email",
                    Password = "password"
                }), CancellationToken.None);
            });

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            uowMock.Verify(u => u.Users.GetUserByEmail(It.IsAny<string>()), Times.Once);
        }
    }
}
