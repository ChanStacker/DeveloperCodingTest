using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;

namespace HackerNewsGateway
{
    public static class JsonSerializerOptionsProvider
    {
        static JsonSerializerOptionsProvider()
        {
            JsonOptions = new JsonOptions
            {
                SerializerOptions = 
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                }
            };

            JsonOptions.SerializerOptions.Converters.Add(new DateTimeConverter());
        }

        public static JsonOptions JsonOptions { get; set; }
    }
}
