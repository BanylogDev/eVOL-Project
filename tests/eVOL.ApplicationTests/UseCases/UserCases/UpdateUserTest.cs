using Moq;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using Microsoft.Extensions.Logging;
using eVOL.Domain.Entities;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.UserCases.Commands.UpdateUser;


namespace eVOL.ApplicationTests.UseCases.UserCases
{
    public class UpdateUserTest
    {
        [Fact]
        public async Task UpdateUser_UpdateUserWithNewData_ReturnsUser()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<UpdateUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var fakeUser = new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "OldName",
                Email = "OldEmail",
                Password = "HashedNewPassword",
            };

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync(fakeUser);

            passwordHasherMock.Setup(p => p.HashPassword("OldPassword")).Returns("HashedNewPassword");

            var sut = new UpdateUserHandler(
                uowMock.Object,
                passwordHasherMock.Object,
                loggerMock.Object
            );

            // Act

            var result = await sut.Handle(new UpdateUserCommand(new UpdateDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "NewName",
                Email = "NewEmail",
                Password = "OldPassword",
                ConfirmPassword = "OldPassword",
            }),CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(fakeUser.Name, result!.Name);
            Assert.Equal(fakeUser.Email, result.Email);

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Never);

            userRepoMock.Verify(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);

            passwordHasherMock.Verify(p => p.HashPassword("OldPassword"), Times.Exactly(2));
        }

        [Fact]

        public async Task UpdateUser_UpdateUserNull_ReturnsNull()
        {
            // Arrange
            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<UpdateUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ReturnsAsync((User?)null);

            var sut = new UpdateUserHandler(
                uowMock.Object,
                passwordHasherMock.Object,
                loggerMock.Object
            );

            // Act

            var result = await sut.Handle(new UpdateUserCommand(new UpdateDTO
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "NewName",
                Email = "NewEmail",
                Password = "OldPassword",
                ConfirmPassword = "OldPassword",
            }), CancellationToken.None);

            // Assert

            Assert.Null(result);
            
            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);

        }

        [Fact]
        public async Task UpdateUser_ThrowException_ReturnNothing()
        {
            // Arrange

            var uowMock = new Mock<IPostgreUnitOfWork>();
            var userRepoMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<UpdateUserHandler>>();

            uowMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            userRepoMock.Setup(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001"))).ThrowsAsync(new Exception("Database error"));

            var sut = new UpdateUserHandler(
                uowMock.Object,
                passwordHasherMock.Object,
                loggerMock.Object
            );

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await sut.Handle(new UpdateUserCommand(new UpdateDTO
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "NewName",
                    Email = "NewEmail",
                    Password = "OldPassword",
                    ConfirmPassword = "OldPassword",
                }), CancellationToken.None);
            });

            uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            uowMock.Verify(u => u.RollbackAsync(), Times.Once);

            userRepoMock.Verify(u => u.GetUserById(Guid.Parse("00000000-0000-0000-0000-000000000001")), Times.Once);
        }
    }
}

