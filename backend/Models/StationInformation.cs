namespace backend.Models
{
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
}
