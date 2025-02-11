using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TicTatToe.Data.Models;
using TicTatToe.Data.Settings;

namespace TicTatToe.Data.Storage;

public class RatingMongoStorage : MongoStorage<Rating>
{
    public RatingMongoStorage(
        IOptions<RatingDatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        Collection = mongoDatabase.GetCollection<Rating>(
            databaseSettings.Value.CollectionName);
    }
}