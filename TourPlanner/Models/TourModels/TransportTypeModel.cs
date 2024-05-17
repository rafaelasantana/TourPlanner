using System.Text.Json;
using System.Text.Json.Serialization;

namespace TourPlanner.Models.TourModels;
public enum TransportTypeModel
{
    Foot = 100,
    Car = 200,
    Truck = 300,
    Bicycle = 400,
    Bike = 500
}

public class TransportTypeConverter : JsonConverter<TransportTypeModel>
{
    public override TransportTypeModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "Foot" => TransportTypeModel.Foot,
            "Car" => TransportTypeModel.Car,
            "Truck" => TransportTypeModel.Truck,
            "Bicycle" => TransportTypeModel.Bicycle,
            "Bike" => TransportTypeModel.Bike,
            _ => throw new JsonException("Unexpected value: " + value)
        };
    }

    public override void Write(Utf8JsonWriter writer, TransportTypeModel value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}