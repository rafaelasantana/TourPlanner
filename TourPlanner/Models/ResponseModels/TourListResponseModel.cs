using System.Text.Json.Serialization;
using TourPlanner.Models.TourModels;

namespace TourPlanner.Models.ResponseModels;

public class TourListResponseModel
{
    [JsonPropertyName("@odata.context")]
    public string? ODataContext { get; set; }

    [JsonPropertyName("value")]
    public List<TourModel>? Tours { get; set; }
}
