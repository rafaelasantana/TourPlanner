using Microsoft.AspNetCore.Components;
using TourPlanner.Models;
using TourPlanner.Models.TourLogModels;
using TourPlanner.Services;
using System.Collections.ObjectModel;
using TourPlanner.Models.TourModels;

namespace TourPlanner.ViewModels.CustomViewModels;

public class HomeViewModel(TourService tourService, TourLogService tourLogService, NavigationManager navigationManager) : ObservableObject
{
    private string _searchQuery = string.Empty;
    private string _searchUrl = string.Empty;
    
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (SetProperty(ref _searchQuery, value))
            {
                // Update the SearchUrl whenever the SearchQuery changes
                SearchUrl = $"/search?query={Uri.EscapeDataString(_searchQuery)}";
            }
        }
    }
    
    public string SearchUrl
    {
        get => _searchUrl;
        private set => SetProperty(ref _searchUrl, value);
    }

    public ObservableCollection<TourModel> BelovedTours { get; set; } = new();
    public ObservableCollection<TourLogModel> LatestLogs { get; set; } = new();

    public async Task InitializeAsync()
    {
        await LoadBelovedToursAsync();
        await LoadLatestLogsAsync();
    }
    private async Task LoadBelovedToursAsync()
    {
        var tours = await tourService.GetTopPopularToursAsync();
        if (tours != null)
        {
            BelovedTours.Clear();
            foreach (var tour in tours)
            {
                BelovedTours.Add(tour);
            }
        }
    }

    private async Task LoadLatestLogsAsync()
    {
        var logs = await tourLogService.GetLatestTourLogsAsync();
        if (logs != null)
        {
            LatestLogs.Clear();
            foreach (var log in logs)
            {
                LatestLogs.Add(log);
            }
        }
    }

    public void NavigateToSearch()
    {
        if (!string.IsNullOrEmpty(SearchQuery))
        {
            navigationManager.NavigateTo(SearchUrl);
        }
    }
}
