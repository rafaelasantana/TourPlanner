using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class TourListViewModel(TourService tourService, TimeFormatService timeFormatService, NavigationManager navigationManager)
    : ObservableObject
{
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
                tour.FormattedEstimatedTime = tour.EstimatedTime.Format();
                tour.PopularityFormatted = PopularityToString(tour.Popularity);
                Tours.Add(tour);
            }
        }
        else
        {
            ErrorMessage = "No tours available.";
        }
    }

    private string PopularityToString(Popularity? popularity) => popularity switch
    {
        Popularity.NotPopular => "Not popular",
        Popularity.SlightlyPopular => "Slightly popular",
        Popularity.ModeratelyPopular => "Moderately popular",
        Popularity.Popular => "Popular",
        Popularity.HighlyPopular => "Highly popular",
        Popularity.ExtremelyPopular => "Extremely popular",
        _ => "No data yet"
    };
    
    public void NavigateToDetail(string tourId)
    {
        var url =$"/tour/details/{tourId}";
        navigationManager.NavigateTo(url);
    }
}