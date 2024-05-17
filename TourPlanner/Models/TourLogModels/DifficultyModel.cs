using System.Text.Json;
using System.Text.Json.Serialization;
using TourPlanner.Models.TourModels;

namespace TourPlanner.Models.TourLogModels;

public enum DifficultyModel
{
    VeryEasy = 500,
    Easy = 400,
    Normal = 300,
    Hard = 200,
    VeryHard = 100
}

public class DifficultyModelConverter : JsonConverter<DifficultyModel>
{
    public override DifficultyModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "VeryHard" => DifficultyModel.VeryHard,
            "Hard" => DifficultyModel.Hard,
            "Normal" => DifficultyModel.Normal,
            "Easy" => DifficultyModel.Easy,
            "VeryEasy" => DifficultyModel.VeryEasy,
            _ => throw new JsonException("Unexpected value: " + value)
        };
    }

    public override void Write(Utf8JsonWriter writer, DifficultyModel value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
    
}