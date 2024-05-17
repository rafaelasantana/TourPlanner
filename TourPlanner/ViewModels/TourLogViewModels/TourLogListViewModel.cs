using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourLogModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourLogViewModels;

public class TourLogListViewModel(TourLogService tourLogService, TimeFormatService timeFormatService, NavigationManager navigationManager)
    : ObservableObject
{
    public TimeFormatService TimeFormatService { get; } = timeFormatService;
    private string _tourId = String.Empty;
    private string? _createTourLogComment;
    private string? _searchText;
    private string _totalTimeFormatted = String.Empty;
    private string? _errorMessage;
    public ObservableCollection<TourLogModel> TourLogs { get; set; } = [];

    
    public string TourId
    {
        get => _tourId;
        set => SetProperty(ref _tourId, value);

    }
    public string? CreateTourLogComment
    {
        get => _createTourLogComment;
        set => SetProperty(ref _createTourLogComment, value);
    }
    
    public string? SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }
    
    public string TotalTimeFormatted
    {
        get => _totalTimeFormatted;
        set => SetProperty(ref _totalTimeFormatted, value);
    }
    
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    
    private string _search = string.Empty;

    public string Search
    {
        get => _search;
        set => SetProperty(ref _search, value);
    }
    
    public async Task InitializeAsync(string? tourId)
    {
        if (tourId != null)
        {
            _tourId = tourId;
            await LoadTourLogsAsync();
        }
    }
    
    private async Task LoadTourLogsAsync()
    {
        if (string.IsNullOrEmpty(_tourId))
        {
            ErrorMessage = "Tour ID is null or empty.";
            return;
        }
        var (logs, errorMessage) = await tourLogService.GetTourLogsAsync(_tourId);
        if (logs != null)
        {
            TourLogs.Clear(); // Clear existing logs
            foreach (var log in logs)
            {
                // Format the total time before adding to the observable collection
                log.FormattedTotalTime = TimeFormatService.ParseIso8601DurationToString(log.TotalTime);
                TourLogs.Add(log);
            }
        }
        else
        {
            ErrorMessage = errorMessage;
        }
    }

    
    public void HandleCreateTourLog()
    {
        if (!string.IsNullOrEmpty(CreateTourLogComment) && !string.IsNullOrEmpty(TourId))
        {
            var url = $"/tour-log/create?comment={Uri.EscapeDataString(CreateTourLogComment)}&tourId={Uri.EscapeDataString(TourId)}";
            navigationManager.NavigateTo(url);
        }
        else
        {
            ErrorMessage = "Please ensure all fields are filled correctly.";
        }
    }

    public void HandleSearchComment()
    {
        _ = LoadTourLogsAsync(); // Reload all logs if needed
        if (string.IsNullOrEmpty(_searchText)) return;
        var filtered = TourLogs.Where(log => log.Comment.Contains(_searchText, StringComparison.OrdinalIgnoreCase)).ToList();
        TourLogs.Clear();
        foreach (var log in filtered)
        {
            TourLogs.Add(log);
        }
    }

    public void FilterLogs(string searchText)
    {

    }

}