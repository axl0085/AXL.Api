using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AXL.Commons
{
    public class DateTimeNullableConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTimeString = reader.GetString();
            return (string.IsNullOrEmpty(dateTimeString) || dateTimeString.Equals("0-0-0")) ? default(DateTime?) : DateTime.Parse(dateTimeString);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
