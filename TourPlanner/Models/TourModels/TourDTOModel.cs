using System.ComponentModel.DataAnnotations;

namespace TourPlanner.Models.TourModels;

public class TourDTOModel
{
    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Transport Type is required.")]
    public TransportTypeModel TransportType { get; set; }
    
    [Required(ErrorMessage = "Starting Point is required.")]
    public string? Start { get; set; }
    
    [Required(ErrorMessage = "End Point is required.")]
    public string? End { get; set; }
}