using impacta_contatos_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace impacta_contatos_api.Services
{
    public class ContactServices
    {
        private readonly IMongoCollection<ContactDocument> _contactCollection;

        private int CalcSkipAmount(int pageNumber, int pageSize) => pageNumber* pageSize;


        public ContactServices(IOptions<DatabaseSettings> contactServices)
        {
            var mongoClient = new MongoClient(contactServices.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(contactServices.Value.DatabaseName);

            _contactCollection = mongoDatabase.GetCollection<ContactDocument>(contactServices.Value.CollectionName);

        }

        public async Task<List<ContactDocument>> GetAsync(int pageNumber, int pageSize, string sortOrder)
        {
            var skipAmount = CalcSkipAmount(pageNumber, pageSize);

            var sortDefinition = sortOrder.ToLower() == "descending" ?
                Builders<ContactDocument>.Sort.Combine(
                    Builders<ContactDocument>.Sort.Descending(contact => contact.CreatedAt),
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.Id)
                ) :
                Builders<ContactDocument>.Sort.Combine(
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.CreatedAt),
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.Id)
                );

            var contacts = await _contactCollection.Find(contact => true)
                                                  .Sort(sortDefinition)
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();

            return contacts;
        }


        public async Task<List<ContactDocument>> GetContactsByDateAsync(DateTime date, int pageNumber, int pageSize, string sortOrder)
        {
            var skipAmount = CalcSkipAmount(pageNumber, pageSize);
            var sortDefinition = sortOrder.ToLower() == "descending" ?
                Builders<ContactDocument>.Sort.Descending(contact => contact.CreatedAt) :
                Builders<ContactDocument>.Sort.Ascending(contact => contact.CreatedAt);

            var startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            var endOfDay = startOfDay.AddDays(1);

            var filter = Builders<ContactDocument>.Filter.Gte(contact => contact.CreatedAt, startOfDay) &
                         Builders<ContactDocument>.Filter.Lt(contact => contact.CreatedAt, endOfDay);

            var contacts = await _contactCollection.Find(filter)
                                                  .Sort(sortDefinition)
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();

            return contacts;
        }



        public async Task<List<ContactDocument>> FindByFieldAsync(string field, string value, int pageNumber, int pageSize, string sortOrder)
        {
            var skipAmount = CalcSkipAmount(pageNumber, pageSize);

            var filter = Builders<ContactDocument>.Filter.Regex(field, new BsonRegularExpression(value, "i"));
            var sortDefinition = sortOrder.ToLower() == "descending" ?
                Builders<ContactDocument>.Sort.Combine(
                    Builders<ContactDocument>.Sort.Descending(contact => contact.CreatedAt),
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.Id)
                ) :
                Builders<ContactDocument>.Sort.Combine(
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.CreatedAt),
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.Id)
                );

            var contacts = await _contactCollection.Find(filter)
                                                  .Sort(sortDefinition)
                                                  .Skip(skipAmount)
                                                  .Limit(pageSize)
                                                  .ToListAsync();

            return contacts;
        }



        public async Task<long> CountAsync() =>
          await _contactCollection.CountDocumentsAsync(FilterDefinition<ContactDocument>.Empty);

        public async Task<long> CountByFieldAsync(string field, string value) =>
          await _contactCollection.CountDocumentsAsync(Builders<ContactDocument>.Filter.Eq(field, value));

        public async Task<long> CountByDateAsync(DateTime date)
        {
            var startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            var endOfDay = startOfDay.AddDays(1);

            var filter = Builders<ContactDocument>.Filter.Gte(contact => contact.CreatedAt, startOfDay) &
                         Builders<ContactDocument>.Filter.Lt(contact => contact.CreatedAt, endOfDay);

            return await _contactCollection.CountDocumentsAsync(filter);
        }
        public async Task<List<ContactDocument>> SearchContactsAsync(string searchString, int pageNumber, int pageSize, string sortOrder)
        {
            var skipAmount = CalcSkipAmount(pageNumber, pageSize);

            var sortDefinition = sortOrder.ToLower() == "descending" ?
                Builders<ContactDocument>.Sort.Combine(
                    Builders<ContactDocument>.Sort.Descending(contact => contact.CreatedAt),
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.Id)
                ) :
                Builders<ContactDocument>.Sort.Combine(
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.CreatedAt),
                    Builders<ContactDocument>.Sort.Ascending(contact => contact.Id)
                );

            var filter = Builders<ContactDocument>.Filter.Or(
                Builders<ContactDocument>.Filter.Regex("Name", new BsonRegularExpression(searchString, "i")),
                Builders<ContactDocument>.Filter.Regex("LegalField", new BsonRegularExpression(searchString, "i")),
                Builders<ContactDocument>.Filter.Regex("Email", new BsonRegularExpression(searchString, "i")),
                Builders<ContactDocument>.Filter.Regex("Description", new BsonRegularExpression(searchString, "i"))
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
            var filter = Builders<ContactDocument>.Filter.Or(
                Builders<ContactDocument>.Filter.Regex("Name", new BsonRegularExpression(searchString, "i")),
                Builders<ContactDocument>.Filter.Regex("LegalField", new BsonRegularExpression(searchString, "i")),
                Builders<ContactDocument>.Filter.Regex("Email", new BsonRegularExpression(searchString, "i")),
                Builders<ContactDocument>.Filter.Regex("Description", new BsonRegularExpression(searchString, "i"))
            );

            return await _contactCollection.CountDocumentsAsync(filter);
        }
        public async Task<ContactDocument> FindOneAsync(string contactId) =>
            await _contactCollection.Find(contact => contact.Id == contactId).FirstOrDefaultAsync();
        public async Task CreateAsync(ContactDocument contactData) =>
            await _contactCollection.InsertOneAsync(contactData);

        public async Task UpdateAsync(ContactDocument contactData) =>
            await _contactCollection.ReplaceOneAsync(contact => contact.Id == contactData.Id, contactData);

        public async Task RemoveAsync(string contactId) =>
            await _contactCollection.DeleteOneAsync(contact => contact.Id == contactId);
    }
}
