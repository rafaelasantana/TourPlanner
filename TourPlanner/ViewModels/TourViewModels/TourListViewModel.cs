using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class TourListViewModel(TourService tourService, TimeFormatService timeFormatService)
    : ObservableObject
{
    public TimeFormatService TimeFormatService { get; } = timeFormatService;
    private string? _errorMessage;
    public ObservableCollection<TourModel> Tours { get; private set; } = [];

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    public async Task InitializeAsync()
    {
        await GetAllToursAsync();
    }
    
    private async Task GetAllToursAsync()
    {
        var tours = await tourService.GetAllToursAsync();
        if (tours != null)
        {
            Tours.Clear(); // Clear existing tours
            foreach (var tour in tours)
            {
                // Format the total time before adding to the observable collection
                tour.FormattedEstimatedTime = TimeFormatService.ParseIso8601DurationToString(tour.EstimatedTime);
                Tours.Add(tour);
            }
        }
        else
        {
            ErrorMessage = "No tours available.";
        }
    }
}