using EpollApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using EpollApi.Services;
using Microsoft.Extensions.Options;

namespace EpollApi.Services
{
    public class MongoDBService
    {

        private readonly IMongoCollection<Poll> _pollsCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _pollsCollection = database.GetCollection<Poll>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<Poll>> GetAsync() 
        {
            return await _pollsCollection.Find(new BsonDocument()).ToListAsync();
        }
        public async Task<Poll> GetByIdAsync(string id)
        {
            var filter = Builders<Poll>.Filter.Eq(p => (p.Id), id);
            return await _pollsCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task VoteOptionAsync(string id, int optionId) 
        {
            var filter = Builders<Poll>.Filter.Eq(p => p.Id, id);
            var update = Builders<Poll>.Update.Inc($"Options.{optionId}.Votes", 1);
            await _pollsCollection.UpdateOneAsync(filter, update);
        }

        public async Task CreateAsync(Poll poll)
        {
            await _pollsCollection.InsertOneAsync(poll);
            return;
        }

    }
}
