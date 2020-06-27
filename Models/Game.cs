using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace GamesApi.Models
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        [JsonProperty("Name")]
        public string GameName { get; set; }

        //public decimal Price { get; set; }
        public Platform Price { get; set; }

        public string Category { get; set; }

        public string Developer { get; set; }

        public string Release { get; set; }
        
        public DateTime CreatedOn { get; set; }
        //public string Created { get; set; }
        public DateTime? UpdatedOn { get; set; } = null;
    }
}
