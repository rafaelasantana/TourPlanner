using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

namespace TourPlanner.Models.TourModels;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? duration = reader.GetString();
        return XmlConvert.ToTimeSpan(duration);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        string duration = XmlConvert.ToString(value);
        writer.WriteStringValue(duration);
    }
}