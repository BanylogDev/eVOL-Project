using eVOL.Application.DTOs.Responses.Admin;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.AdminCases.Queries.AdminGetUser
{
    public class AdminGetUserHandler : IRequestHandler<AdminGetUserQuery, GetUserAdminResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<AdminGetUserHandler> _logger;

        public AdminGetUserHandler(IPostgreUnitOfWork uow, ILogger<AdminGetUserHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<GetUserAdminResponse> Handle(AdminGetUserQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Admin -> Started geting user with id: {UserId}", request.Id);

            var user = await _uow.Users.GetUserById(request.Id, ct);

            if (user is null)
            {
                _logger.LogWarning("Admin -> User with id: {UserId} not found! ", request.Id);
                return new GetUserAdminResponse
                {
                    IsSuccess = false,
                    Error = "User not found!"
                };
            }

            _logger.LogInformation("Admin -> Ended getting user with id: {UserId}, Success", request.Id);
            return new GetUserAdminResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Address = user.Address,
                Money = user.Money,
                ChatGroups = user.ChatGroups,
                SupportTickets = user.SupportTickets,
                ClaimedTickets = user.ClaimedTickets,
                CreatedAt = user.CreatedAt,
                IsSuccess = true
            };
        }
    }
}
