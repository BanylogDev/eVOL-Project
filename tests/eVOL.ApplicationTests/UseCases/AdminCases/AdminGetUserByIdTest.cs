using Moq;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Domain.Entities;
using Microsoft.Extensions.Logging;
using eVOL.Application.Features.AdminCases.Queries.AdminGetUser;


namespace eVOL.ApplicationTests.UseCases.AdminCases
{
    public class AdminGetUserByIdTest
    {
        [Fact]
        public async Task AdminGetUserById_GetUserById_ReturnUser()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<AdminGetUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            var fakeUser = new User
            {
                UserId = 1
            };

            userRepoMock.Setup(u => u.GetUserById(1)).ReturnsAsync(fakeUser);

            var sut = new AdminGetUserHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new AdminGetUserQuery(1), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeUser.UserId, result.UserId);

            userRepoMock.Verify(u => u.GetUserById(1), Times.Once);
        }

        [Fact]
        public async Task AdminGetUserById_GetUserNullById_ReturnNull()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<AdminGetUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            userRepoMock.Setup(u => u.GetUserById(1)).ReturnsAsync((User?)null);

            var sut = new AdminGetUserHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new AdminGetUserQuery(1), CancellationToken.None);

            // Assert

            Assert.Null(result);

            userRepoMock.Verify(u => u.GetUserById(1), Times.Once);
        }
    }
}
