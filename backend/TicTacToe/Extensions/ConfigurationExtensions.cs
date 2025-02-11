using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.Common.CQRS;
using TicTacToe.Common.Helpers;
using TicTacToe.Common.Helpers.Abstractions;
using TicTacToe.Consumers;
using TicTacToe.Handlers;
using TicTacToe.Requests;
using TicTacToe.Responses;
using TicTacToe.Services;
using TicTacToe.Services.Abstractions;

namespace TicTacToe.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddJwtConfigured(this IServiceCollection services)
    {
        var config =
            services.BuildServiceProvider()
                .GetService<IConfiguration>()!;

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });

        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<JwtOptions>()
            .AddScoped<IPasswordHasherService, PasswordHasherService>()
            .AddScoped<IJwtService, JwtService>()
            .AddScoped<IAuthService, AuthService>();
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        return services
            .AddScoped<IHandler<SignUpRequest, IResult>, SignUpHandler>()
            .AddScoped<IHandler<SignInRequest, IResult>, SignInHandler>()
            .AddScoped<IHandler<RefreshRequest, IResult>, RefreshHandler>()
            .AddScoped<IHandler<GetRatingRequest, IResult>, GetRatingHandler>()
            .AddScoped<IHandler<CreateGameRequest, IResult>, CreateGameHandler>()
            .AddScoped<IHandler<ViewGameRoomListRequest, IResult>, ViewGameRoomListHandler>()
            .AddScoped<IHandler<EnterGameRoomRequest, BaseResponse>, EnterGameRoomHandler>()
            .AddScoped<IHandler<LeaveGameRoomRequest, BaseResponse>, LeaveGameRoomHandler>()
            .AddScoped<IHandler<PlayerMadeTurnRequest, BaseResponse>, PlayerMadeTurnHandler>();
    }
    
    public static IServiceCollection AddMassTransitConfigured(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMassTransit(x =>
        {
            var configuration =
                serviceCollection.BuildServiceProvider()
                    .GetService<IConfiguration>()!;

            x.AddConsumer<GameStartedConsumer>()
                .Endpoint(e => e.Name = nameof(GameStartedConsumer));
            x.AddConsumer<GameEndedConsumer>()
                .Endpoint(e => e.Name = nameof(GameEndedConsumer));
            x.AddConsumer<PlayerJoinedConsumer>()
                .Endpoint(e => e.Name = nameof(PlayerJoinedConsumer));
            x.AddConsumer<PlayerLeftConsumer>()
                .Endpoint(e => e.Name = nameof(PlayerLeftConsumer));
            x.AddConsumer<PlayerMadeTurnConsumer>()
                .Endpoint(e => e.Name = nameof(PlayerMadeTurnConsumer));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VHost"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        serviceCollection.AddScoped<GameStartedConsumer>();
        serviceCollection.AddScoped<GameEndedConsumer>();
        serviceCollection.AddScoped<PlayerJoinedConsumer>();
        serviceCollection.AddScoped<PlayerLeftConsumer>();
        serviceCollection.AddScoped<PlayerMadeTurnConsumer>();
        return serviceCollection;
    }
}