using System.Text.Json.Serialization;

namespace TourPlanner.Models.TourLogModels;

public class TourLogDTOModel
{
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
    
    [JsonPropertyName("difficulty")]
    public DifficultyModel Difficulty { get; set; }
    
    [JsonPropertyName("totalDistanceMeters")]
    public double TotalDistanceMeters { get; set; }
    
    [JsonPropertyName("totalTime")]
    public string TotalTime { get; set; } = string.Empty;
    
    [JsonPropertyName("rating")]
    public int Rating { get; set; }
}