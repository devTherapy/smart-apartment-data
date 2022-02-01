using System.Text.Json.Serialization;

namespace Smart_Data.Domain.Models
{
    public class Managements
    {
        [JsonPropertyName("mgmt")]
        public ManagementData Management { get; set; }
    }

    public class ManagementData
    {
        [JsonPropertyName("mgmtID")]
        public long ManagementId { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
        public string Keywords { get; set; }
    }
}
