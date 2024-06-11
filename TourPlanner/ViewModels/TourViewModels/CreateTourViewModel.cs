using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class CreateTourViewModel(TourService tourService, NavigationManager navigationManager) : ObservableObject
{
    private string _description = string.Empty;
    private string _name = string.Empty;
    private TransportTypeModel _transportType = TransportTypeModel.Foot;
    private string _start = string.Empty;
    private string _end = string.Empty;
    private string? _errorMessage = string.Empty;
    
    [Required(ErrorMessage = "Description is required.")]
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    [Required(ErrorMessage = "Name is required.")]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    
    [Required(ErrorMessage = "Transport type is required.")]
    [JsonConverter(typeof(TransportTypeConverter))]
    public TransportTypeModel TransportType
    {
        get => _transportType;
        set => SetProperty(ref _transportType, value);
    }

    [Required(ErrorMessage = "Starting point is required.")]
    public string Start
    {
        get => _start;
        set => SetProperty(ref _start, value);
    }
    
    [Required(ErrorMessage = "End point is required.")]
    public string End
    {
        get => _end;
        set => SetProperty(ref _end, value);
    }
    
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }
    public async Task HandleValidSubmit()
    {
        var newTour = new TourDTOModel
        {
            Description = Description,
            Name = Name,
            TransportType = TransportType,
            Start = Start,
            End = End
        };

        var (createdTour, errorMessage) = await tourService.CreateTourAsync(newTour);

        if (createdTour != null)
        {
            navigationManager.NavigateTo($"/tour/details/{createdTour.Id}");
        }
        else
        {
            ErrorMessage = errorMessage;
        }
    }
}
