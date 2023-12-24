using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace impacta_contatos_api.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId Id { get; set; }

        public required string Name { get; set; }

        public required string LegalField { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required string Image { get; set; }

        public required string Description { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
