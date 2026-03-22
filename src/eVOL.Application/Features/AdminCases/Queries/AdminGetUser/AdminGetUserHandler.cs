using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.AdminCases.Queries.AdminGetUser
{
    public class AdminGetUserHandler : IRequestHandler<AdminGetUserQuery, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<AdminGetUserHandler> _logger;

        public AdminGetUserHandler(IPostgreUnitOfWork uow, ILogger<AdminGetUserHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<User?> Handle(AdminGetUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin -> Started geting user with id: {UserId}", request.Id);

            var user = await _uow.Users.GetUserById(request.Id);

            if (user == null)
            {
                _logger.LogWarning("Admin -> User with id: {UserId} not found! ", request.Id);
                return null;
            }

            _logger.LogInformation("Admin -> Ended getting user with id: {UserId}, Success", request.Id);
            return user;
        }
    }
}
