using eVOL.Application.Mappings;
using eVOL.Application.Messaging.Interfaces;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Infrastructure.Data;
using eVOL.Infrastructure.Persistence;
using eVOL.Infrastructure.Repositories;
using eVOL.Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using StackExchange.Redis;
using System.Text;
using System.Threading.RateLimiting;

namespace eVOL.API.Configuration
{
    public static class DependecyInjection
    {

        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {

            var redisConnectionString = configuration["CacheSettings:RedisConnection"];
            var options = ConfigurationOptions.Parse(redisConnectionString);
            options.AbortOnConnectFail = false;

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(options)
            );

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(options)
            );

            var redisConnection = configuration["CacheSettings:RedisConnection"];
            services.AddSignalR().AddStackExchangeRedis(redisConnection, opts =>
            {
                opts.Configuration.ChannelPrefix = "evol.signalr";
            });

            return services;
        }

        public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")
                )
            );

            services.AddSingleton<MongoDbContext>(sp =>
            {
                var config = configuration;
                var connectionString = config.GetConnectionString("MongoDB");
                return new MongoDbContext(connectionString, "eVOLChat");
            });

            services.AddScoped<IClientSessionHandle>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.StartSession();
            });

            return services;
        }

        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
            });


            return services;
        }

        public static IServiceCollection AddCorsService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithOrigins("http://localhost:5001")
                          .AllowCredentials();
                });
            });

            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var config = new TypeAdapterConfig();
            config.Scan(typeof(UserMappings).Assembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        public static IServiceCollection AddScopedUseCases(this IServiceCollection services)
        {
            // Services

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Messaging

            services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

            // Repositories

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatGroupRepository, ChatGroupRepository>();
            services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            // Unit's Of Work

            services.AddScoped<IPostgreUnitOfWork, PostgreUnitOfWork>();
            services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();


            // SeedData

            services.AddScoped<SeedData>();


            return services;
        }

        public static IServiceCollection AddRateLimiterService(this IServiceCollection services)
        {
            services.AddRateLimiter(o =>
            {

                o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                             ?? context.Connection.RemoteIpAddress?.ToString()
                             ?? "unknown";

                    if (!string.IsNullOrEmpty(ip) && ip.Contains(','))
                    {
                        ip = ip.Split(',')[0].Trim();
                    }


                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ip,
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 60,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });
            });

            return services;
        }
    }

    
}
