using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourLogModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourLogViewModels;

public class EditTourLogViewModel(TourLogService tourLogService, TourService tourService, TimeFormatService timeFormatService, NavigationManager navigationManager)
    : ObservableObject
{
    public TimeFormatService TimeFormatService { get; } = timeFormatService;
    private string _tourLogId = string.Empty;
    private string _tourId = string.Empty;
    private string _tourName = string.Empty;
    private string _comment = string.Empty;
    private DifficultyModel _difficulty;
    private double _totalDistanceMeters;
    private int _hours;
    private int _minutes;
    private string _totalTime = string.Empty;
    private int _rating;
    private string? _errorMessage = string.Empty;
    private string? _successMessage = string.Empty;
    private bool _showConfirmation = false;
    
    public string TourId
    {
        get => _tourId;
        set => SetProperty(ref _tourId, value);
    }

    public string TourLogId
    {
        get => _tourLogId;
        set => SetProperty(ref _tourLogId, value);
    }

    public string TourName
    {
        get => _tourName;
        set => SetProperty(ref _tourName, value);
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
            SetProperty(ref _hours, value);
            UpdateTotalTime();
        }
    }

    public int Minutes
    {
        get => _minutes;
        set
        {
            if (value is < 0 or >= 60) return;
            SetProperty(ref _minutes, value);
            UpdateTotalTime();
        }
    }

    [Required(ErrorMessage = "Time must be greater than 00:00:00.")]
    public string TotalTime
    {
        get => _totalTime;
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

    public string? SuccessMessage
    {
        get => _successMessage;
        private set => SetProperty(ref _successMessage, value);
    }

    public bool ShowConfirmation
    {
        get => _showConfirmation;
        set => SetProperty(ref _showConfirmation, value);
    }

    private void UpdateTotalTime()
    {
        _totalTime = TimeFormatService.FormatStandardDuration(Hours, Minutes);
        OnPropertyChanged(nameof(TotalTime));
    }

    public void RequestConfirmation()
    {
        ShowConfirmation = true;
    }

    public void HandleConfirmation(bool confirmed)
    {
        if (confirmed)
        {
            _ = DeleteTourLogAsync();
        }
        ShowConfirmation = false;
    }

    private async Task DeleteTourLogAsync()
    {
        if (string.IsNullOrEmpty(TourLogId))
        {
            ErrorMessage = "Tour Log ID is invalid or not provided.";
            return;
        }

        var (isSuccess, errorMessage) = await tourLogService.DeleteTourLogAsync(TourLogId);
        if (isSuccess)
        {
            ErrorMessage = "Tour Log successfully deleted.";
            navigationManager.NavigateTo($"/tour/details/{TourId}");
        }
        else
        {
            ErrorMessage = errorMessage;
        }
    }

    public async Task InitializeAsync(string? tourLogId)
    {
        await LoadTourLogAsync(tourLogId);
    }

    private async Task LoadTourLogAsync(string? tourLogId)
    {
        if (!string.IsNullOrEmpty(tourLogId))
        {
            var (tourLog, errorMessage) = await tourLogService.GetTourLogByIdAsync(tourLogId);
            if (tourLog != null)
            {
                TourId = tourLog.TourId;
                TourLogId = tourLog.Id;
                Comment = tourLog.Comment;
                Difficulty = tourLog.Difficulty;
                TotalDistanceMeters = tourLog.TotalDistanceMeters;
                Rating = tourLog.Rating;

                Console.WriteLine($"Total time in viewmodel: ", tourLog.TotalTime);

                // Parse standard duration format
                var (hours, minutes) = TimeFormatService.ParseIso8601DurationToTuple(tourLog.TotalTime);
                Hours = hours;
                Minutes = minutes;

                await SetTourName(TourId);
            }
            else
            {
                ErrorMessage = errorMessage;
            }
        }
    }

    private async Task SetTourName(string tourId)
    {
        var tourResult = await tourService.GetTourByIdAsync(tourId);
        if (tourResult.tour != null)
        {
            TourName = tourResult.tour.Name;
        }
    }

    public async Task UpdateTourLogAsync()
    {
        if (string.IsNullOrEmpty(TourLogId))
        {
            ErrorMessage = "Tour Log ID is invalid or not provided.";
            return;
        }

        var tourLogDto = new TourLogDTOModel
        {
            Comment = Comment,
            Difficulty = Difficulty,
            TotalDistanceMeters = TotalDistanceMeters,
            TotalTime = TotalTime,
            Rating = Rating
        };

        var (isSuccess, errorMessage) = await tourLogService.UpdateTourLogAsync(tourLogDto, TourLogId);
        if (isSuccess)
        {
            SuccessMessage = "Tour Log successfully updated.";
            ErrorMessage = string.Empty; // Clear any existing error messages
        }
        else
        {
            ErrorMessage = errorMessage;
        }
    }
}
