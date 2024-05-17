using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public partial class TourDetailsViewModel(TourService tourService, TimeFormatService timeFormatService, NavigationManager navigationManager) : ObservableObject
{
    public TimeFormatService TimeFormatService { get; } = timeFormatService;
    private TourModel _tour = new TourModel();
    private string? _errorMessage;
    private string? _tourId;
    private string _estimatedTimeFormatted = String.Empty;
    public string EditUrl => navigationManager.ToAbsoluteUri($"/tour/edit/{TourId}").ToString();
    
    public string? TourId
    {
        get => _tourId;
        init
        {
            if (SetProperty(ref _tourId, value))
            {
                OnPropertyChanged(nameof(EditUrl));
            }
        }
    }

    public string EstimatedTimeFormatted
    {
        get => _estimatedTimeFormatted;
        set => SetProperty(ref _estimatedTimeFormatted, value);
    }
    
    public TourModel Tour
    {
        get => _tour;
        set => SetProperty(ref _tour, value);
    }
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    public async Task InitializeAsync()
    {
        await LoadTourAsync(TourId);
    }
    
    private async Task LoadTourAsync(string? tourId)
    {
        var result = await tourService.GetTourByIdAsync(tourId);
        if (result.tour != null)
        {
            _tour = result.tour;
            _estimatedTimeFormatted = TimeFormatService.ParseIso8601DurationToString(_tour.EstimatedTime);
            
        }
        else
        {
            ErrorMessage = result.errorMessage;
        }
    }
}