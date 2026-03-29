using eVOL.Domain.RepositoriesInteraces;
using Moq;
using eVOL.Application.ServicesInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.UserCases.Commands.RefreshToken;
using Microsoft.Extensions.Options;
using eVOL.Application.Options;


namespace eVOL.ApplicationTests.UseCases.UserCases
{
    public class RefreshTokenTest
    {
        [Fact]
        public async Task RefreshToken_RefreshJWTToken_ReturnsTokenDto( )
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var jwtServiceMock = new Mock<IJwtService>();
            var optionsMock = new Mock<IOptions<JwtOptions>>();
            var loggerMock = new Mock<ILogger<RefreshTokenHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "username",
                RefreshToken = "validRefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(1)
            };

            uowMock.Setup(u => u.Users.GetUserByName(It.IsAny<string>())).ReturnsAsync(fakeUser);

            jwtServiceMock.Setup(j => j.GetPrincipalFromExpiredToken("expiredAccessToken", It.IsAny<IOptions<JwtOptions>>()))
                .Returns(new System.Security.Claims.ClaimsPrincipal(
                    new System.Security.Claims.ClaimsIdentity(
                        new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "username") }
                    )
                ));

            jwtServiceMock.Setup(j => j.GenerateJwtToken(fakeUser, It.IsAny<IOptions<JwtOptions>>())).Returns("newAccessToken");

            var sut = new RefreshTokenHandler(jwtServiceMock.Object, uowMock.Object, optionsMock.Object, loggerMock.Object);

            var tokenDto = new TokenDTO
            {
                AccessToken = "expiredAccessToken",
                RefreshToken = "validRefreshToken"
            };

            // Act

            var result = await sut.Handle(new RefreshTokenCommand(tokenDto), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal("newAccessToken", result.AccessToken);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            uowMock.Verify(u => u.Users.GetUserByName(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_RefreshTokenNull_ReturnsNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var jwtServiceMock = new Mock<IJwtService>();
            var optionsMock = new Mock<IOptions<JwtOptions>>();
            var loggerMock = new Mock<ILogger<RefreshTokenHandler>>();

            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            jwtServiceMock.Setup(j => j.GetPrincipalFromExpiredToken("expiredAccessToken", It.IsAny<IOptions<JwtOptions>>()))
                .Returns((System.Security.Claims.ClaimsPrincipal?)null);

            var sut = new RefreshTokenHandler(jwtServiceMock.Object, uowMock.Object, optionsMock.Object, loggerMock.Object);

            var tokenDto = new TokenDTO
            {
                AccessToken = "expiredAccessToken",
                RefreshToken = "invalidRefreshToken"
            };

            // Act

            var result = await sut.Handle(new RefreshTokenCommand(tokenDto), CancellationToken.None);

            // Assert

            Assert.Null(result);
            
            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_ThrowException_RollbackTransaction()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var authRepoMock = new Mock<IAuthRepository>();
            var jwtServiceMock = new Mock<IJwtService>();
            var optionsMock = new Mock<IOptions<JwtOptions>>();
            var loggerMock = new Mock<ILogger<RefreshTokenHandler>>();

            uowMock.Setup(u => u.Auth).Returns(authRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            jwtServiceMock.Setup(j => j.GetPrincipalFromExpiredToken("expiredAccessToken", It.IsAny<IOptions<JwtOptions>>()))
                .Throws(new Exception("Test exception"));

            var sut = new RefreshTokenHandler(jwtServiceMock.Object, uowMock.Object, optionsMock.Object, loggerMock.Object);

            var tokenDto = new TokenDTO
            {
                AccessToken = "expiredAccessToken",
                RefreshToken = "validRefreshToken"
            };

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () => await sut.Handle(new RefreshTokenCommand(tokenDto), CancellationToken.None));

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);
        }
    }
}
