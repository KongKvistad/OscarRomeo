using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Station
    {
        [JsonPropertyName("station_id")]
        public string StationId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("num_bikes_available")]
        public int NumBikesAvailable { get; set; }

        [JsonPropertyName("num_docks_available")]
        public int NumDocksAvailable { get; set; }
    }
}
