using Microsoft.AspNetCore.Components;

namespace TourPlanner.ViewModels.CustomViewModels;

public class HomeViewModel(NavigationManager navigationManager) : ObservableObject
{
    private string _searchQuery = string.Empty;
    
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

    private string _searchUrl = string.Empty;
    public string SearchUrl
    {
        get => _searchUrl;
        private set => SetProperty(ref _searchUrl, value);
    }

    public void NavigateToSearch()
    {
        if (!string.IsNullOrEmpty(SearchQuery))
        {
            navigationManager.NavigateTo(SearchUrl);
        }
    }
} 