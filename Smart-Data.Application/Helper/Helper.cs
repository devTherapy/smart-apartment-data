using System.Text.Json;

namespace Smart_Data.Application.Helper
{
    public static class Helper
    {
        public static string Serialize<T>(this T data) where T : class
        {

           return JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        }

        public static T Deserialize<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions {PropertyNamingPolicy =JsonNamingPolicy.CamelCase });
        }
    }
}
