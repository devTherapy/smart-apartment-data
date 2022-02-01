using System.Text.Json.Serialization;

namespace Smart_Data.Domain.Models
{
    public  class Properties
    {
        [JsonPropertyName("property")]
        public PropertyData Property { get; set; }
    }

    public class PropertyData
    {
        [JsonPropertyName("propertyID")]
        public long PropertyID { get; set; }
        public string Name { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
        [JsonPropertyName("lat")]
        public double Lattitude { get; set; }
        [JsonPropertyName("lng")]
        public double Longittude { get; set; }
        public string Keywords { get; set; }

    }
}
