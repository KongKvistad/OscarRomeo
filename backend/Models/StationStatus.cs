namespace backend.Models
{
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
}
