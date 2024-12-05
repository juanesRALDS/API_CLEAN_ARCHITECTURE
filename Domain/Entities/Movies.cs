using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_completa_mongodb_net_6_0.Domain.Entities
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("plot")]
        public string Plot { get; set; } = string.Empty;

        [BsonElement("genres")]
        public List<string> Genres { get; set; } = new List<string>();

        [BsonElement("runtime")]
        public int Runtime { get; set; }

        [BsonElement("rated")]
        public string Rated { get; set; } = string.Empty;

        [BsonElement("cast")]
        public List<string> Cast { get; set; } = new List<string>();

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("fullplot")]
        public string FullPlot { get; set; } = string.Empty;

        [BsonElement("languages")]
        public List<string> Languages { get; set; } = new List<string>();

        [BsonElement("released")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Released { get; set; }

        [BsonElement("directors")]
        public List<string> Directors { get; set; } = new List<string>();

        [BsonElement("writers")]
        public List<string> Writers { get; set; } = new List<string>();

        [BsonElement("awards")]
        public Awards Awards { get; set; } = new Awards();

        [BsonElement("lastupdated")]
        public string LastUpdated { get; set; } = string.Empty;

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("imdb")]
        public Imdb Imdb { get; set; } = new Imdb();

        [BsonElement("countries")]
        public List<string> Countries { get; set; } = new List<string>();

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("tomatoes")]
        public Tomatoes Tomatoes { get; set; } = new Tomatoes();
    }

    public class Imdb
    {
        [BsonElement("rating")]
        public double Rating { get; set; }

        [BsonElement("votes")]
        public int Votes { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }
    }

    public class Awards
    {
        [BsonElement("wins")]
        public int Wins { get; set; }

        [BsonElement("nominations")]
        public int Nominations { get; set; }

        [BsonElement("text")]
        public string Text { get; set; } = string.Empty;
    }

    public class Tomatoes
    {
        [BsonElement("viewer")]
        public Viewer Viewer { get; set; } = new Viewer();

        [BsonElement("production")]
        public string Production { get; set; } = string.Empty;

        [BsonElement("lastUpdated")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastUpdated { get; set; }
    }

    public class Viewer
    {
        [BsonElement("rating")]
        public double Rating { get; set; }

        [BsonElement("numReviews")]
        public int NumReviews { get; set; }

        [BsonElement("meter")]
        public int Meter { get; set; }
    }
}
