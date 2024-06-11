using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using TourPlanner.Models.TourLogModels;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourLogViewModels;

public class CreateTourLogViewModel(TourLogService tourLogService, TourService tourService, NavigationManager navigationManager) : ObservableObject
{
    private string _selectedTourId = string.Empty;
    private List<TourModel> _tours = [];
    private string _comment = string.Empty;
    private DifficultyModel _difficulty = DifficultyModel.VeryEasy;
    private double _totalDistanceMeters;
    private int _hours;
    private int _minutes;
    private string _totalTime = string.Empty;
    private int _rating = 5;
    private string? _errorMessage = string.Empty;
    
    [Required(ErrorMessage = "Tour is required.")]
    public string SelectedTourId
    {
        get => _selectedTourId;
        set => SetProperty(ref _selectedTourId, value);
    }
    
    public List<TourModel> Tours
    {
        get => _tours;
        private set => SetProperty(ref _tours, value);
    }
    
    [Required(ErrorMessage = "Comment is required.")]
    public string Comment 
    {
        get => _comment;
        set => SetProperty(ref _comment, value);
    }
    
    [Required(ErrorMessage = "Difficulty is required.")]
    public DifficultyModel Difficulty 
    {
        get => _difficulty;
        set => SetProperty(ref _difficulty, value);
    }
    
    [Required(ErrorMessage = "Total Distance is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Distance must be a positive number.")]
    public double TotalDistanceMeters 
    {
        get => _totalDistanceMeters;
        set => SetProperty(ref _totalDistanceMeters, value);
    }
    
    public int Hours
    {
        get => _hours;
        set
        {
            if (value is < 0 or >= 24) return;
            _hours = value;
            UpdateTotalTime();
        }
    }

    public int Minutes
    {
        get => _minutes;
        set
        {
            if (value is < 0 or >= 60) return;
            _minutes = value;
            UpdateTotalTime();
        }
    }
    
    [Required(ErrorMessage = "Time must be greater than 00:00:00.")]
    public string TotalTime
    {
        get => $"{Hours:D2}:{Minutes:D2}:00";
        set => SetProperty(ref _totalTime, value);
    }
    
    [Required(ErrorMessage = "Rating is required.")]
    public int Rating 
    {
        get => _rating;
        set => SetProperty(ref _rating, value);
    }
    
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }
    private void UpdateTotalTime()
    {
        TotalTime = $"{Hours:D2}:{Minutes:D2}:00";
        OnPropertyChanged(nameof(TotalTime));
    }
    public async Task InitializeAsync()
    {
        ParseQueryParameters();
        await LoadToursAsync();
    }
    private void ParseQueryParameters()
    {
        var uri = new Uri(navigationManager.Uri);
        var queryParameters = QueryHelpers.ParseQuery(uri.Query);
    
        // Safely get the 'tourId' from the query parameters if it exists
        if (queryParameters.TryGetValue("tourId", out var tourIds) && tourIds.Count > 0)
        {
            SelectedTourId = tourIds.First()!;
        }

        // Safely get the 'comment' from the query parameters if it exists
        if (queryParameters.TryGetValue("comment", out var comments) && comments.Count > 0)
        {
            Comment = comments.First()!;
        }
    }


    private async Task LoadToursAsync()
    {
        var tours = await tourService.GetAllToursAsync();
        if (tours != null)
            Tours = [..tours];
    }
    
    public async Task<string?> CreateTourLog()
    {
        var newTourLog = new TourLogDTOModel
        {
            Comment = _comment,
            Difficulty = _difficulty,
            TotalTime = TotalTime,
            Rating = _rating,
            TotalDistanceMeters = _totalDistanceMeters
        };

        var (isSuccess, errorMessage) = await tourLogService.CreateTourLogAsync(newTourLog, _selectedTourId);
        if (isSuccess)
        {
            navigationManager.NavigateTo($"/tour/details/{_selectedTourId}");
            return null;
        }
        else
        {
            ErrorMessage = errorMessage;
            return errorMessage;
        }
    }

}