using eVOL.Application.DTOs.Responses.User;
using eVOL.Domain.RepositoriesInteraces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResponse?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<GetUserHandler> _logger;

        public GetUserHandler(IPostgreUnitOfWork uow, ILogger<GetUserHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<GetUserResponse?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetUserById(request.Id);

            if (user == null)
            {
                _logger.LogWarning("GetUserUseCase: User with ID {UserId} not found.", request.Id);
                return null;
            }

            _logger.LogInformation("GetUserUseCase: Successfully retrieved User with ID {UserId}.", request.Id);

            return user.Adapt<GetUserResponse>();
        }
    }
}
