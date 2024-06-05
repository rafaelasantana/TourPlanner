using System.Text.Json.Serialization;
using TourPlanner.Models.TourModels;

namespace TourPlanner.Models.TourLogModels;

public class TourLogModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("comment")]
    public string Comment { get; set; } = string.Empty;
    
    [JsonPropertyName("difficulty")]
    [JsonConverter(typeof(DifficultyModelConverter))]
    public DifficultyModel Difficulty { get; set; }
    
    [JsonPropertyName("difficultyInt")]
    public int DifficultyInt { get; set; }
    
    [JsonPropertyName("totalDistanceMeters")]
    public double TotalDistanceMeters { get; set; }
    
    [JsonPropertyName("totalTime")]
    public string TotalTime { get; set; } = string.Empty;
    
    // Extra for formatted time
    public string? FormattedTotalTime { get; set; } = string.Empty; 
    
    // Extra for a descriptive rating based on the numerical value
    public string RatingDescription => Rating switch
    {
        1 => "Awful",
        2 => "Bad",
        3 => "OK",
        4 => "Good",
        5 => "Great",
        _ => "Unknown" // Default case if the rating is out of expected range
    };
    
    [JsonPropertyName("rating")]
    public int Rating { get; set; }
    
    [JsonPropertyName("createdOn")]
    public DateTime CreatedOn { get; set; }
    
    [JsonPropertyName("tourId")]
    public string TourId { get; set; } = string.Empty;
    
    [JsonPropertyName("tour")]
    public TourModel Tour { get; set; } = new TourModel();
}