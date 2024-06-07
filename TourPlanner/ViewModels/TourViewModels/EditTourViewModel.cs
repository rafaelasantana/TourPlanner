using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class EditTourViewModel(TourService tourService, NavigationManager navigationManager)
    : ObservableObject
{
    private TourModel _tour = new TourModel();
    private string? _errorMessage;
    private bool _showConfirmation = false;

    private string _description = string.Empty;
    private string _name = string.Empty;
    private TransportTypeModel _transportType;
    private string _start = string.Empty;
    private string _end = string.Empty;

    private TourModel Tour
    {
        get => _tour;
        set => SetProperty(ref _tour, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    
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

    public bool ShowConfirmation
    {
        get => _showConfirmation;
        set => SetProperty(ref _showConfirmation, value);
    }
    public async Task InitializeAsync(string? tourId)
    {
        await LoadTourAsync(tourId);
    }
    
    public void RequestConfirmation()
    {
        _showConfirmation = true;
    }

    public void HandleConfirmation(bool confirmed)
    {
        if (confirmed)
        {
            _ = DeleteTourAsync();
        }
        _showConfirmation = false;
    }


    private async Task DeleteTourAsync()
    {
        if (string.IsNullOrEmpty(Tour.Id.ToString()))
        {
            ErrorMessage = "Tour ID is invalid.";
            return;
        }

        var (isSuccess, errorMessage) = await tourService.DeleteTourAsync(Tour.Id.ToString());
        if (isSuccess)
        {
            navigationManager.NavigateTo("/tours");
        }
        else
        {
            ErrorMessage = errorMessage;
        }
    }

    private async Task LoadTourAsync(string? tourId)
    {
        var result = await tourService.GetTourByIdAsync(tourId);
        if (result.tour != null)
        {
            Tour = result.tour;
            _name = Tour.Name;
            _transportType = Tour.TransportType;
            _start = Tour.Start!;
            _end = Tour.End!;
            _description = Tour.Description!;
        }
        else
        {
            ErrorMessage = result.errorMessage;
        }
    }
    
    public async Task<(bool isSuccess, string errorMessage)> UpdateTourAsync()
    {
        try
        {
            
            // Prepare the tour object with updated values
            _tour.Name = Name;
            _tour.TransportType = TransportType;
            _tour.Start = Start;
            _tour.End = End;
            _tour.Description = Description;
        
            var (isSuccess, errorContent) = await tourService.UpdateTourAsync(_tour);
            if (isSuccess)
            {
                // Redirect to the specific tour's detail page, using the tour's ID
                navigationManager.NavigateTo($"/tour/details/{_tour.Id}");
                return (true, "");
            }

            // Handle potential errors during update
            if (errorContent == null) 
                return (false, "Updated failed, no error message in the response.");
        
            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
            return (false, errorData?.Error?.Message ?? "An error occurred during update.");
        }
        catch (Exception ex)
        {
            return (false, $"Exception when updating tour: {ex.Message}");
        }
    }

}
