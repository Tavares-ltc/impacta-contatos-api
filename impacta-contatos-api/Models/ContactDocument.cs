using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace impacta_contatos_api.Models
{
    public class ContactDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null;

        public string Name { get; set; }

        public string LegalField { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}