using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(IPostgreUnitOfWork uow, IPasswordHasher passwordHasher, ILogger<UpdateUserHandler> logger)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting UpdateUserUseCase for User ID: {UserId}", request.Dto.Id);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserById(request.Dto.Id);

                if (user == null)
                {
                    _logger.LogWarning("UpdateUserUseCase failed: User not found.");
                    return null;
                }

                if (user.Password != _passwordHasher.HashPassword(request.Dto.Password) || request.Dto.Password != request.Dto.ConfirmPassword)
                {
                    _logger.LogWarning("UpdateUserUseCase failed: Password mismatch.");
                    return null;
                }

                _logger.LogInformation("Updating User ID: {UserId}", request.Dto.Id);

                user.Name = request.Dto.Name;
                user.Email = request.Dto.Email;
                user.Password = _passwordHasher.HashPassword(request.Dto.Password);

                await _uow.CommitAsync();

                _logger.LogInformation("UpdateUserUseCase completed successfully for User ID: {UserId}", request.Dto.Id);

                return user;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "UpdateUserUseCase failed and rolled back for User ID: {UserId}", request.Dto.Id);
                throw;
            }
        }
    }
}
