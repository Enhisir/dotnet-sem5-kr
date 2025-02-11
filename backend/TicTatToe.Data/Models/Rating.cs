using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicTatToe.Data.Models;

public class Rating
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserName { get; set; }
    public double Value { get; set; }
}