using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

public class StationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StationService> _logger;

    public StationService(HttpClient httpClient, ILogger<StationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    private async Task<T> FetchDataFromApiAsync<T>(string url)
    {
        var response = await _httpClient.GetStringAsync(url);
        _logger.LogInformation($"Fetched data from {url}: {response}");
        return JsonSerializer.Deserialize<T>(response);
    }

    public async Task<List<Station>> GetMergedStationDataAsync()
    {
        var stationInfo = await FetchDataFromApiAsync<StationInformation>("https://gbfs.urbansharing.com/oslobysykkel.no/station_information.json");
        var stationStatus = await FetchDataFromApiAsync<StationStatus>("https://gbfs.urbansharing.com/oslobysykkel.no/station_status.json");

        var statusDict = new Dictionary<string, StationStatus.StationStatusData>();
        foreach (var status in stationStatus.data.stations)
        {
            statusDict[status.station_id] = status;
        }

        var mergedStations = new List<Station>();
        foreach (var station in stationInfo.data.stations)
        {
            if (statusDict.TryGetValue(station.station_id, out var status))
            {
                mergedStations.Add(new Station
                {
                    StationId = station.station_id,
                    Name = station.name,
                    Lat = station.lat,
                    Lon = station.lon,
                    Capacity = station.capacity,
                    NumBikesAvailable = status.num_bikes_available,
                    NumDocksAvailable = status.num_docks_available
                });
            }
            else
            {
                // Log if a status is missing for a station
                _logger.LogWarning($"Missing status for station {station.station_id}");
            }
        }

        _logger.LogInformation($"Merged station data: {JsonSerializer.Serialize(mergedStations)}");
        return mergedStations;
    }
}

public class StationInformation
{
    public StationInformationData data { get; set; }

    public class StationInformationData
    {
        public List<StationInfo> stations { get; set; }
    }

    public class StationInfo
    {
        public string station_id { get; set; }
        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public int capacity { get; set; }
    }
}

public class StationStatus
{
    public StationStatusDataContainer data { get; set; }

    public class StationStatusDataContainer
    {
        public List<StationStatusData> stations { get; set; }
    }

    public class StationStatusData
    {
        public string station_id { get; set; }
        public int num_bikes_available { get; set; }
        public int num_docks_available { get; set; }
    }
}

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
