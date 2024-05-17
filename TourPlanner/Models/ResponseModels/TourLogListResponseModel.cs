using System.Text.Json.Serialization;
using TourPlanner.Models.TourLogModels;

namespace TourPlanner.Models.ResponseModels;

public class TourLogListResponseModel
{
    [JsonPropertyName("@odata.context")]
    public string? ODataContext { get; set; }

    [JsonPropertyName("value")]
    public List<TourLogModel>? TourLogs { get; set; }
}