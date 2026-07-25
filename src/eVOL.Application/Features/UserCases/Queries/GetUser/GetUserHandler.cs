using eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<GetUserHandler> _logger;

        public GetUserHandler(IPostgreUnitOfWork uow, ILogger<GetUserHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken ct)
        {
            var user = await _uow.Users.GetUserById(request.Id, ct);

            if (user is null)
            {
                _logger.LogWarning("GetUserUseCase: User with ID {UserId} not found.", request.Id);
                return new GetUserResponse
                {
                    IsSuccess = false,
                    Error = $"User with ID {request.Id} not found."
                };
            }

            _logger.LogInformation("GetUserUseCase: Successfully retrieved User with ID {UserId}.", request.Id);

            return new GetUserResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Address = user.Address,
                Money = user.Money,
                CreatedAt = user.CreatedAt,
                IsSuccess = true,
            };
        }
    }
}
