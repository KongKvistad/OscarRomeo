using Newtonsoft.Json;


public class StationService
{
    private readonly HttpClient _httpClient;

    public StationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> FetchDataFromApiAsync<T>(string url)
    {
        var response = await _httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<T>(response);
    }

    public async Task<StationInformation> GetStationInformationAsync()
    {
        var url = "https://gbfs.urbansharing.com/oslobysykkel.no/station_information.json";
        return await FetchDataFromApiAsync<StationInformation>(url);
    }
}

public class StationInformation
{
    // Define properties based on the JSON structure of the response
    public Data data { get; set; }

    public class Data
    {
        public List<Station> stations { get; set; }

        public class Station
        {
            public string station_id { get; set; }
            public string name { get; set; }
            public double lat { get; set; }
            public double lon { get; set; }
            public int capacity { get; set; }
        }
    }
}
