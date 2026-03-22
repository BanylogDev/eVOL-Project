using eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser;
using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.FeaturesCases.Admin.Commands.AdminDeleteUser
{
    public class AdminDeleteUserHandler : IRequestHandler<AdminDeleteUserCommand, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<AdminDeleteUserHandler> _logger;

        public AdminDeleteUserHandler(IPostgreUnitOfWork uow, ILogger<AdminDeleteUserHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<User?> Handle(AdminDeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin -> Started Deletion of user {UserId}", request.Id);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserById(request.Id);

                if (user == null)
                {

                    _logger.LogWarning("Admin -> Error, user doesn't exist with id: {UserId}", request.Id);

                    return null;
                }

                _logger.LogInformation("Admin -> Deleting user with id: {UserId}", request.Id);

                _uow.Users.RemoveUser(user);
                await _uow.CommitAsync();

                _logger.LogInformation("Admin -> Success, Ended Deletion of user {UserId}", request.Id);

                return user;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "Admin -> Error, Something went wrong during deletion of user with id: {UserId}", request.Id);
                throw;
            }
        }
    }
}
