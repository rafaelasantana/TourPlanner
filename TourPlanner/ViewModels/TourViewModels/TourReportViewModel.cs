using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class TourReportViewModel(TourService tourService, NavigationManager navigationManager)
    : ObservableObject
{
    private readonly NavigationManager _navigationManager = navigationManager;

    private string _reportType = "single";
    private string _selectedTourId = string.Empty;
    private string? _errorMessage;
    private List<TourModel> _tours = [];

    [Required(ErrorMessage = "Report type is required.")]
    public string ReportType
    {
        get => _reportType;
        set => SetProperty(ref _reportType, value);
    }

    [Required(ErrorMessage = "Tour is required.")]
    public string SelectedTourId
    {
        get => _selectedTourId;
        set => SetProperty(ref _selectedTourId, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    public List<TourModel> Tours
    {
        get => _tours;
        set => SetProperty(ref _tours, value);
    }

    public async Task InitializeAsync()
    {
        await LoadToursAsync();
    }

    private async Task LoadToursAsync()
    {
        var tours = await tourService.GetAllToursAsync();
        if (tours != null)
        {
            Tours = tours;
        }
    }

    public Task GenerateReport()
    {
        return Task.CompletedTask;
    }
}
