using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class TourDetailsViewModel(TourService tourService, TimeFormatService timeFormatService, NavigationManager navigationManager) : ObservableObject
{
    public TimeFormatService TimeFormatService { get; } = timeFormatService;
    private TourModel _tour = new TourModel();
    private string? _errorMessage;
    private readonly string? _tourId;
    private string _estimatedTimeFormatted = string.Empty;
    private string _popularityFormatted = string.Empty;
    private string _childFriendlinessFormatted= string.Empty;

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
    
    public string PopularityFormatted
    {
        get => _popularityFormatted;
        set => SetProperty(ref _popularityFormatted, value);
    }
    
    public string ChildFriendlinessFormatted
    {
        get => _childFriendlinessFormatted;
        set => SetProperty(ref _childFriendlinessFormatted, value);
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
            _estimatedTimeFormatted = _tour.EstimatedTime.Format();
            _popularityFormatted = PopularityToString(_tour.Popularity);
            _childFriendlinessFormatted = ChildFriendlinessToString(_tour.ChildFriendliness);
        }
        else
        {
            ErrorMessage = result.errorMessage;
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

    private string ChildFriendlinessToString(bool? childFriendliness) => childFriendliness switch
        {
            true => "Child-friendly",
            false => "Not child-friendly",
            _ => "No data yet"
        };
}