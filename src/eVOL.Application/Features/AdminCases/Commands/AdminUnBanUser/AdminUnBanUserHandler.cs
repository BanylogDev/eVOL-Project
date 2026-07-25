using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.AdminCases.Commands.AdminUnBanUser
{
    public class AdminUnBanUserHandler : IRequestHandler<AdminUnBanUserCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<AdminUnBanUserHandler> _logger;

        public AdminUnBanUserHandler(IPostgreUnitOfWork uow, ILogger<AdminUnBanUserHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(AdminUnBanUserCommand request, CancellationToken ct)
        {
            _logger.LogInformation("AdminUnBanUserHandler started for user with Id: {Id}", request.Id);


            if (!await _uow.Admin.UnBanUser(request.Id, ct))
            {
                _logger.LogError("Failed to ban user.");
                return new ResultResponse { IsSuccess = false, Error = "Failed to ban user." };
            }

            _logger.LogInformation("Unbanned user with id: {UserId} successfully", request.Id);

            return new ResultResponse
            {
                IsSuccess = true
            };
        }
    }
}
