using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class TourListViewModel(TourService tourService, TimeFormatService timeFormatService, NavigationManager navigationManager)
    : ObservableObject
{
    public TimeFormatService TimeFormatService { get; } = timeFormatService;
    private string? _errorMessage;
    private string _popularityFormatted = string.Empty;

    public ObservableCollection<TourModel> Tours { get; private set; } = [];

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    
    public string PopularityFormatted
    {
        get => _popularityFormatted;
        set => SetProperty(ref _popularityFormatted, value);
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
                tour.PopularityFormatted = tour.Popularity?.ToString() ?? "No data yet";
                Tours.Add(tour);
            }
        }
        else
        {
            ErrorMessage = "No tours available.";
        }
    }
    
    public void NavigateToDetail(string tourId)
    {
        var url =$"/tour/details/{tourId}";
        navigationManager.NavigateTo(url);
    }
}