using Moq;
using eVOL.Domain.RepositoriesInteraces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById;

namespace eVOL.ApplicationTests.UseCases.SupportTicketCases
{
    public class GetSupportTicketByIdTest
    {
        [Fact]
        public async Task GetSupportTicketById_GetSupportTicketSuccessfully_ReturnSupportTicket()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<GetSupportTicketByIdHandler>>();

            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            var fakeSupportTicket = new SupportTicket
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            supportTicketRepoMock.Setup(s => s.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync(fakeSupportTicket);

            var sut = new GetSupportTicketByIdHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new GetSupportTicketByIdQuery(Guid.Parse("00000000-0000-0000-0000-000000000001")), CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeSupportTicket.Id, result.Id);

            supportTicketRepoMock.Verify(s =>  s.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once());
        }

        [Fact]
        public async Task GetSupportTicketById_SupportTicketNull_ReturnNull()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var supportTicketRepoMock = new Mock<ISupportTicketRepository>();
            var loggerMock = new Mock<ILogger<GetSupportTicketByIdHandler>>();

            uowMock.Setup(u => u.SupportTicket).Returns(supportTicketRepoMock.Object);

            var fakeSupportTicket = new SupportTicket
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            supportTicketRepoMock.Setup(s => s.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync((SupportTicket?)null);

            var sut = new GetSupportTicketByIdHandler(uowMock.Object, loggerMock.Object);

            // Act

            var result = await sut.Handle(new GetSupportTicketByIdQuery(Guid.Parse("00000000-0000-0000-0000-000000000001")), CancellationToken.None);

            // Assert

            Assert.Null(result);

            supportTicketRepoMock.Verify(s => s.GetSupportTicketById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once());
        }
    }
}
