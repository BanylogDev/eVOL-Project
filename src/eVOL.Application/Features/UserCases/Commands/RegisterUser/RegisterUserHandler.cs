
using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(IPostgreUnitOfWork uow, IPasswordHasher passwordHasher, ILogger<RegisterUserHandler> logger)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(RegisterUserCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting RegisterUserUseCase for Name: {Name}, Email: {Email}", request.Dto.Name, request.Dto.Email);

            var existingEmail = await _uow.Users.GetUserIdByEmail(request.Dto.Email, ct);

            if (existingEmail is not null)
            {
                _logger.LogWarning("RegisterUserUseCase failed: Email already exists. Email: {Email}", request.Dto.Email);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Email already exists."
                };
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Dto.Password);

            var newAddress = new Address
            (
                request.Dto.Country,
                request.Dto.City,
                request.Dto.AddressName,
                request.Dto.AddressNumber
            );

            var newMoney = new Money(
                request.Dto.Balance,
                request.Dto.Currency);


            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Name = request.Dto.Name,
                Email = request.Dto.Email,
                Password = hashedPassword,
                Address = newAddress,
                Role = "User",
                Money = newMoney,
                CreatedAt = DateTime.UtcNow,
            };

            _logger.LogInformation("Registering new user: Name: {Name}, Email: {Email}", request.Dto.Name, request.Dto.Email);

            if (!await _uow.Auth.Register(newUser, ct))
            {
                _logger.LogError("RegisterUserUseCase failed, something went wrong!");
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "An error occurred while processing your request. Please try again later."
                };
            }

            await _uow.CommitAsync();

            _logger.LogInformation("RegisterUserUseCase completed successfully for Name: {Name}, Email: {Email}", request.Dto.Name, request.Dto.Email);

            return new ResultResponse
            {
                IsSuccess = true
            };

        }
    }
}
