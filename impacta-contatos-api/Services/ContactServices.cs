using impacta_contatos_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace impacta_contatos_api.Services
{
    public class ContactServices
    {
        private readonly IMongoCollection<Contact> _contactCollection;

        private int CalcSkipAmount(int pageNumber, int pageSize) => pageNumber* pageSize;


        public ContactServices(IOptions<DatabaseSettings> contactServices)
        {
            var mongoClient = new MongoClient(contactServices.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(contactServices.Value.DatabaseName);

            _contactCollection = mongoDatabase.GetCollection<Contact>(contactServices.Value.CollectionName);

        }

        public async Task<List<Contact>> GetAsync(int pageNumber, int pageSize, string sortOrder)
        {
            var skipAmount = CalcSkipAmount(pageNumber, pageSize);
            var sortDefinition = sortOrder.ToLower() == "descending" ?
                Builders<Contact>.Sort.Descending(contact => contact.CreatedAt) :
                Builders<Contact>.Sort.Ascending(contact => contact.CreatedAt);

            var contacts = await _contactCollection.Find(contact => true)
                                                  .Sort(sortDefinition)
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();

            return contacts;
        }

        public async Task<List<Contact>> GetContactsByDateAsync(DateTime date, int pageNumber, int pageSize, string sortOrder)
        {
            var skipAmount = CalcSkipAmount(pageNumber, pageSize);
            var sortDefinition = sortOrder.ToLower() == "descending" ?
                Builders<Contact>.Sort.Descending(contact => contact.CreatedAt) :
                Builders<Contact>.Sort.Ascending(contact => contact.CreatedAt);

            var startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            var endOfDay = startOfDay.AddDays(1);

            var filter = Builders<Contact>.Filter.Gte(contact => contact.CreatedAt, startOfDay) &
                         Builders<Contact>.Filter.Lt(contact => contact.CreatedAt, endOfDay);

            var contacts = await _contactCollection.Find(filter)
                                                  .Sort(sortDefinition)
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();

            return contacts;
        }



        public async Task<List<Contact>> FindByFieldAsync(string field, string value, int pageNumber, int pageSize, string sortOrder)
        {
            var skipAmount = CalcSkipAmount(pageNumber, pageSize);

            var filter = Builders<Contact>.Filter.Regex(field, new BsonRegularExpression(value, "i"));
            var sortDefinition = sortOrder.ToLower() == "descending" ?
                Builders<Contact>.Sort.Descending(contact => contact.CreatedAt) :
                Builders<Contact>.Sort.Ascending(contact => contact.CreatedAt);

            var contacts = await _contactCollection.Find(filter)
                                                  .Sort(sortDefinition)
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();

            return contacts;
        }

        public async Task<long> CountAsync() =>
          await _contactCollection.CountDocumentsAsync(FilterDefinition<Contact>.Empty);

        public async Task<long> CountByFieldAsync(string field, string value) =>
          await _contactCollection.CountDocumentsAsync(Builders<Contact>.Filter.Eq(field, value));

        public async Task<long> CountByDateAsync(DateTime date)
        {
            var startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            var endOfDay = startOfDay.AddDays(1);

            var filter = Builders<Contact>.Filter.Gte(contact => contact.CreatedAt, startOfDay) &
                         Builders<Contact>.Filter.Lt(contact => contact.CreatedAt, endOfDay);

            return await _contactCollection.CountDocumentsAsync(filter);
        }
        public async Task<List<Contact>> SearchContactsAsync(string searchString, int pageNumber, int pageSize, string sortOrder)
        {
            var skipAmount = CalcSkipAmount(pageNumber, pageSize);
            var sortDefinition = sortOrder.ToLower() == "descending" ?
                Builders<Contact>.Sort.Descending(contact => contact.CreatedAt) :
                Builders<Contact>.Sort.Ascending(contact => contact.CreatedAt);

            var filter = Builders<Contact>.Filter.Or(
                Builders<Contact>.Filter.Regex("Name", new BsonRegularExpression(searchString, "i")),
                Builders<Contact>.Filter.Regex("LegalField", new BsonRegularExpression(searchString, "i")),
                Builders<Contact>.Filter.Regex("Email", new BsonRegularExpression(searchString, "i")),
                Builders<Contact>.Filter.Regex("Description", new BsonRegularExpression(searchString, "i"))
            );

            var contacts = await _contactCollection.Find(filter)
                                                  .Sort(sortDefinition)
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();
            return contacts;
        }

        public async Task<long> SearchCount(string searchString)
        {
            var filter = Builders<Contact>.Filter.Or(
                Builders<Contact>.Filter.Regex("Name", new BsonRegularExpression(searchString, "i")),
                Builders<Contact>.Filter.Regex("LegalField", new BsonRegularExpression(searchString, "i")),
                Builders<Contact>.Filter.Regex("Email", new BsonRegularExpression(searchString, "i")),
                Builders<Contact>.Filter.Regex("Description", new BsonRegularExpression(searchString, "i"))
            );

            return await _contactCollection.CountDocumentsAsync(filter);
        }
        public async Task<Contact> FindOneAsync(string contactId) =>
            await _contactCollection.Find(contact => contact.Id == contactId).FirstOrDefaultAsync();
        public async Task CreateAsync(Contact contactData) =>
            await _contactCollection.InsertOneAsync(contactData);

        public async Task UpdateAsync(Contact contactData) =>
            await _contactCollection.ReplaceOneAsync(contact => contact.Id == contactData.Id, contactData);

        public async Task RemoveAsync(string contactId) =>
            await _contactCollection.DeleteOneAsync(contact => contact.Id == contactId);
    }
}
