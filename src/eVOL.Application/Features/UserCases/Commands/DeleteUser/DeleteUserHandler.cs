using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(IPostgreUnitOfWork uow, IPasswordHasher passwordHasher, ILogger<DeleteUserHandler> logger)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<User?> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting DeleteUserUseCase for User ID: {UserId}", request.Dto.Id);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserById(request.Dto.Id);

                if (user == null ||
                   !_passwordHasher.VerifyPassword(request.Dto.Password, user.Password))
                {
                    _logger.LogWarning("DeleteUserUseCase failed: User not found or password mismatch.");
                    return null;
                }

                _logger.LogInformation("Deleting User ID: {UserId}", request.Dto.Id);

                _uow.Users.RemoveUser(user);
                await _uow.CommitAsync();

                _logger.LogInformation("DeleteUserUseCase completed successfully for User ID: {UserId}", request.Dto.Id);

                return user;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "DeleteUserUseCase failed and rolled back for User ID: {UserId}", request.Dto.Id);
                throw;
            }
        }
    }
}
