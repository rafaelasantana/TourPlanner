using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourLogModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourLogViewModels;

public class TourLogListViewModel(TourLogService tourLogService)
    : ObservableObject
{
    private string _tourId = string.Empty;
    private string? _searchText;
    private string? _errorMessage;
    public ObservableCollection<TourLogModel> TourLogs { get; set; } = [];

    
    public string TourId
    {
        get => _tourId;
        set => SetProperty(ref _tourId, value);

    }

    public string? SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
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
    
    public async Task SearchAsync()
    {
        await LoadTourLogsAsync(); // Load all logs
        if (!string.IsNullOrEmpty(SearchText))
        {
            var filteredLogs = TourLogs.Where(log => log.Comment.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
            TourLogs.Clear();
            foreach (var log in filteredLogs)
            {
                TourLogs.Add(log);
            }
        }
    }

}