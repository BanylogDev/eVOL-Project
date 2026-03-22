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
                Id = 1,
                Name = "TestGroup",
                OwnerId = 2,
            };

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(1)).ReturnsAsync(fakeChatGroup);

            var sut = new GetChatGroupByIdHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new GetChatGroupByIdQuery(1), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeChatGroup.Name, result.Name);

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(1), Times.Once);
        }

        [Fact]
        public async Task GetChatGroupById_GetNullChatGroup_ReturnsNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var chatGroupRepoMock = new Mock<IChatGroupRepository>();
            var loggerMock = new Mock<ILogger<GetChatGroupByIdHandler>>();

            uowMock.Setup(u => u.ChatGroup).Returns(chatGroupRepoMock.Object);

            chatGroupRepoMock.Setup(c => c.GetChatGroupById(1)).ReturnsAsync((ChatGroup?)null);

            var sut = new GetChatGroupByIdHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new GetChatGroupByIdQuery(1), CancellationToken.None);

            // Assert

            Assert.Null(result);

            chatGroupRepoMock.Verify(c => c.GetChatGroupById(1), Times.Once);
        }
    }
}
