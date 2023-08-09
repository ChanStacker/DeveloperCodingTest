using System.Text.Json;
using System.Text.Json.Serialization;

namespace HackerNewsGateway
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string _dateFormat = @"yyyy-MM-ddTHH:mm:ss+00:00";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            string formattedDate = value.ToString(_dateFormat);
            writer.WriteStringValue(formattedDate);
        }
    }
}