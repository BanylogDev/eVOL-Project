using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById;


namespace eVOL.ApplicationTests.UseCases.ChatGroupCases
{
    public class GetChatGroupByIdTest
    {
        [Fact]
        public async Task GetChatGroupById_GetChatGroupWithId_ReturnsChatGroup()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<GetChatGroupByIdHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            var fakeChatGroup = new ChatGroup
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "TestGroup",
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).ReturnsAsync(fakeChatGroup);

            var sut = new GetChatGroupByIdHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new GetChatGroupByIdQuery(Guid.Parse("00000000-0000-0000-0000-000000000000")), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeChatGroup.Name, result.Name);

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Times.Once);
        }

        [Fact]
        public async Task GetChatGroupById_GetNullChatGroup_ReturnsNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<GetChatGroupByIdHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).ReturnsAsync((ChatGroup?)null);

            var sut = new GetChatGroupByIdHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new GetChatGroupByIdQuery(Guid.Parse("00000000-0000-0000-0000-000000000000")), CancellationToken.None);

            // Assert

            Assert.Null(result);

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Times.Once);
        }
    }
}
