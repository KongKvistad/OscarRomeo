using System.Text.Json;
using backend.Models;

public class StationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StationService> _logger;

    public StationService(HttpClient httpClient, ILogger<StationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public virtual async Task<T> FetchDataFromApiAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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






