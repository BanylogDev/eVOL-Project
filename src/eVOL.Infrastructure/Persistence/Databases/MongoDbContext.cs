using eVOL.Domain.Entities;
using MongoDB.Driver;

namespace eVOL.Infrastructure.Persistence.Databases
{
    public sealed class MongoDbContext
    {
        public IMongoCollection<ChatMessage> ChatMessages { get; }

        public MongoDbContext(IMongoClient client, string databaseName)
        {
            var database = client.GetDatabase(databaseName);

            ChatMessages = database.GetCollection<ChatMessage>("ChatMessages");
        }
    }
}