using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EpollApi.Models
{
    public class Poll
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;

        [BsonElement("options")]
        [JsonPropertyName("options")]
        public List<Option> Options { get; set; } = null!;
    }
}
