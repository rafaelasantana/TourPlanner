using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TourPlanner.Models.TourLogModels;

namespace TourPlanner.Models.TourModels;

public enum Popularity
{
    NotPopular = 100,
    SlightlyPopular = 200,
    ModeratelyPopular = 300,
    Popular = 400,
    HighlyPopular = 500,
    ExtremelyPopular = 600
}

public class TourModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }
    
    [JsonPropertyName("name")]
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("transportType")]
    [Required(ErrorMessage = "Transport type is required.")]
    [JsonConverter(typeof(TransportTypeConverter))]
    public TransportTypeModel TransportType { get; set; }

    [JsonPropertyName("start")]
    [Required(ErrorMessage = "Starting point is required.")]
    public string? Start { get; set; }
    
    [JsonPropertyName("startCoordinates")]
    public CoordinatesModel StartCoordinates { get; set; } = new();
    
    [JsonPropertyName("end")]
    [Required(ErrorMessage = "End point is required.")]
    public string? End { get; set; }
    
    [JsonPropertyName("endCoordinates")]
    public CoordinatesModel EndCoordinates { get; set; } = new();
    
    [JsonPropertyName("distanceMeters")]
    public double DistanceMeters { get; set; }
    
    [JsonPropertyName("estimatedTime")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan EstimatedTime { get; set; }
    
    // Extra for formatted time
    public string? FormattedEstimatedTime { get; set; } = string.Empty;
    
    [JsonPropertyName("popularity")]
    public Popularity? Popularity { get; set; }
    
    public string? PopularityFormatted { get; set; }
    
    [JsonPropertyName("childFriendliness")]
    public bool? ChildFriendliness { get; set; }
    
    [JsonPropertyName("createdOn")]
    public DateTime CreatedOn { get; set; }
    
    [JsonPropertyName("tourLogs")]
    public List<TourLogModel>? TourLogs { get; set; }
    
}
