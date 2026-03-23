using eVOL.Application.DTOs.Responses.User;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Domain.ValueObjects;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse?>
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

        public async Task<RegisterUserResponse?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting RegisterUserUseCase for Name: {Name}, Email: {Email}", request.Dto.Name, request.Dto.Email);

            await _uow.BeginTransactionAsync();

            try
            {
                var existingName = await _uow.Users.GetUserByName(request.Dto.Name);
                var existingEmail = await _uow.Users.GetUserByEmail(request.Dto.Email);

                if (existingName != null || existingEmail != null)
                {
                    _logger.LogWarning("RegisterUserUseCase failed: Name or Email already exists. Name: {Name}, Email: {Email}", request.Dto.Name, request.Dto.Email);
                    return null;
                }


                _logger.LogInformation("Hashing password for Name: {Name}", request.Dto.Name);

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

                await _uow.Auth.Register(newUser);
                await _uow.CommitAsync();

                _logger.LogInformation("RegisterUserUseCase completed successfully for Name: {Name}, Email: {Email}", request.Dto.Name, request.Dto.Email);

                return newUser.Adapt<RegisterUserResponse>();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "RegisterUserUseCase failed and rolled back for Name: {Name}, Email: {Email}", request.Dto.Name, request.Dto.Email);
                throw;
            }
        }
    }
}
