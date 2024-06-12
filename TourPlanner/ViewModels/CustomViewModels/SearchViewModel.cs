using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using TourPlanner.Services;
using TourPlanner.Models;

namespace TourPlanner.ViewModels.CustomViewModels;

public class SearchViewModel(TourService tourService, TourLogService tourLogService, NavigationManager navigationManager)
    : ObservableObject
{
    private string _searchQuery = string.Empty;
    public ObservableCollection<SearchResultModel> SearchResults { get; set; } = [];

    public string SearchQuery
    {
        get => _searchQuery;
        set => SetProperty(ref _searchQuery, value);

    }
    
    public async Task InitializeAsync()
    {
        // Read query parameter
        var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("query", out var query))
        {
            SearchQuery = query;
            await SearchAsync();
        }
    }
    public async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            return;
        }

        SearchResults.Clear();
        await SearchToursAsync();
        await SearchTourLogsAsync();
    }
    private async Task SearchToursAsync()
    {
        var (tours, errorMessage) = await tourService.SearchToursAsync(SearchQuery);

        if (tours != null)
        {
            foreach (var tour in tours)
            {
                SearchResults.Add(new SearchResultModel(
                    tourId: tour.Id,
                    title: tour.Name,
                    type: "Tour",
                    createdOn: tour.CreatedOn
                ));
            }
        }
        else
        {
            // TODO Handle the error (show a message, log it, etc.)
            Console.WriteLine(errorMessage);
        }
    }

    private async Task SearchTourLogsAsync()
    {
        var (logs, errorMessage) = await tourLogService.SearchTourLogsAsync(SearchQuery);

        if (logs != null)
        {
            foreach (var log in logs)
            {
                SearchResults.Add(new SearchResultModel(
                    tourId: log.TourId,
                    title: log.Comment  + " - " + log.Tour.Name,
                    type: "Tour Log",
                    createdOn: log.CreatedOn
                ));
            }
        } 
        else
        {
            // TODO Handle the error (show a message, log it, etc.)
            Console.WriteLine(errorMessage);
        }
    }
    
    public void NavigateToDetail(SearchResultModel result)
    {
        var url =$"/tour/details/{result.TourId}";
        navigationManager.NavigateTo(url);
    }
}