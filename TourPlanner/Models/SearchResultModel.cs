namespace TourPlanner.Models;

public class SearchResultModel(string tourId, string title, string type, DateTime createdOn)
{
    public string TourId { get; set; } = tourId;
    public string Title { get; set; } = title;
    public string Type { get; set; } = type;
    public DateTime CreatedOn { get; set; } = createdOn;
}