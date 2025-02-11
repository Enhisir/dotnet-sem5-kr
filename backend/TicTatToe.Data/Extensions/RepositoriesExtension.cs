using Microsoft.Extensions.DependencyInjection;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTatToe.Data.Extensions;

public static class RepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IRepository<User>, StandardRepository<User>>()
            .AddScoped<IRepository<GameRoom>, StandardRepository<GameRoom>>()
            .AddScoped<IRepository<GameRoomPublic>, StandardRepository<GameRoomPublic>>()
            .AddScoped<IRepository<SystemChatMessage>, StandardRepository<SystemChatMessage>>()
            .AddScoped<IHasTransactions, TransactionManager>();
    }
}