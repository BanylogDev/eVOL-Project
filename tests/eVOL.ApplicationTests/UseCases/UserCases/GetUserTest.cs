using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using Mapster;
using eVOL.Application.DTOs.Responses.User;
using eVOL.Application.Features.UserCases.Queries.GetUser;

namespace eVOL.ApplicationTests.UseCases.UserCases
{
    public class GetUserTest
    {
        [Fact]
        public async Task GetUser_GetUserById_ReturnMappedUserResponse()
        {
            //Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<GetUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync(fakeUser);

            var sut = new GetUserHandler(uowMock.Object, loggerMock.Object);

            //Act

            var result = await sut.Handle(new GetUserQuery(Guid.Parse("00000000-0000-0000-0000-000000000001")), CancellationToken.None);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(fakeUser.UserId, result.UserId);

            userRepoMock.Verify(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);

        }

        [Fact]
        public async Task GetUser_GetNullUserById_ReturnMapperdUserResponseNull()
        {
            //Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<GetUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync((User?)null);

            var sut = new GetUserHandler(uowMock.Object, loggerMock.Object);

            //Act

            var result = await sut.Handle(new GetUserQuery(Guid.Parse("00000000-0000-0000-0000-000000000001")), CancellationToken.None);

            //Assert

            Assert.Null(result);

            userRepoMock.Verify(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);
        }
    }
}
