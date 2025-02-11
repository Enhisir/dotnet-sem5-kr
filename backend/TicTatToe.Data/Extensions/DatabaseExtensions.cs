using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using TicTatToe.Data.Models;
using TicTatToe.Data.Settings;
using TicTatToe.Data.Storage;


namespace TicTatToe.Data.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDbContextConfigured(this IServiceCollection services)
        => services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(
                services
                    .BuildServiceProvider()
                    .GetService<IConfiguration>()!
                    .GetConnectionString("PostgreSQL"), b => b.MigrationsAssembly("TicTacToe")));
    
    public static IServiceCollection AddMongoConfigured(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        var configuration = 
            services
                .BuildServiceProvider()
                .GetService<IConfiguration>()!
                .GetSection("RatingDatabaseSettings");
        return services
            .Configure<RatingDatabaseSettings>(x =>
            {
                x.DatabaseName = configuration.GetSection("DatabaseName").Value!;
                x.CollectionName = configuration.GetSection("CollectionName").Value!;
                x.ConnectionString = configuration.GetSection("ConnectionString").Value!;
            })
            .AddSingleton<MongoStorage<Rating>, RatingMongoStorage>();
    }
}