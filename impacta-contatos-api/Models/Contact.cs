using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace impacta_contatos_api.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string Name { get; set; }

        public required string LegalField { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required string Image {  get; set; }

        public required string Description { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
