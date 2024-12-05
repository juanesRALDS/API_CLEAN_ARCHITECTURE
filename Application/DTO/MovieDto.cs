using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_completa_mongodb_net_6_0.Application.DTO;

public class MovieDto
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("genres")]
    public List<string> Genres { get; set; } = new List<string>();

    [BsonElement("runtime")]
    public int Runtime { get; set; }

    [BsonElement("plot")]
    public string Plot { get; set; } = string.Empty;

    [BsonElement("cast")]
    public List<string> Cast { get; set; } = new List<string>();

    [BsonElement("directors")]
    public List<string> Directors { get; set; } = new List<string>();

    [BsonElement("year")]
    public int Year { get; set; }

    [BsonElement("imdb")]
    public ImdbDto Imdb { get; set; } = new ImdbDto();
}

public class ImdbDto
{
    public double Rating { get; set; }
    public int Votes { get; set; }
    public string Id { get; set; }
}

