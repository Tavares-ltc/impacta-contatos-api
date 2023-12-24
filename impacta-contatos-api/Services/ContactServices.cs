using impacta_contatos_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace impacta_contatos_api.Services
{
    public class ContactServices
    {
        private readonly IMongoCollection<Contact> _contactCollection;

        public ContactServices(IOptions<DatabaseSettings> contactServices)
        {
            var mongoClient = new MongoClient(contactServices.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(contactServices.Value.DatabaseName);

            _contactCollection = mongoDatabase.GetCollection<Contact>(contactServices.Value.CollectionName);

        }

        public async Task<List<Contact>> GetAsync(int pageNumber, int pageSize)
        {
            var skipAmount = pageNumber * pageSize;
            var contacts = await _contactCollection.Find(contact => true)
                                                  .Sort(Builders<Contact>.Sort.Ascending(contact => contact.CreatedAt))
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();

            return contacts;
        }


        public async Task<List<Contact>> FindByFieldAsync(string field, string value, int pageNumber, int pageSize)
        {
            var skipAmount = pageNumber * pageSize;
            var contacts = await _contactCollection.Find(Builders<Contact>.Filter.Eq(field, value))
                                                  .Sort(Builders<Contact>.Sort.Ascending(contact => contact.CreatedAt))
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();
            return contacts;
        }
        public async Task<Contact> GetAsync(ObjectId contactId) =>
            await _contactCollection.Find(contact => contact.Id == contactId).FirstOrDefaultAsync();

        public async Task CreateAsync(Contact contactData) =>
            await _contactCollection.InsertOneAsync(contactData);

        public async Task UpdateAsync(ObjectId contactId, Contact contactData) =>
            await _contactCollection.ReplaceOneAsync(contact => contact.Id == contactId, contactData);

        public async Task RemoveAsync(ObjectId contactId) =>
            await _contactCollection.DeleteOneAsync(contact => contact.Id == contactId);
    }
}
