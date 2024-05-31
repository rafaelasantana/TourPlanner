using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;
public class ManageTourViewModel(TourService tourService, NavigationManager navigationManager)
    : ObservableObject
{
    private string _selectedTourId = string.Empty;
    private List<TourModel> _tours = [];
    private string? _errorMessage = string.Empty;
    private bool _isExportSuccessful;
    private byte[]? _exportedFileContent;

    [Required(ErrorMessage = "Tour is required.")]
    public string SelectedTourId
    {
        get => _selectedTourId;
        set => SetProperty(ref _selectedTourId, value);
    }

    public List<TourModel> Tours
    {
        get => _tours;
        set => SetProperty(ref _tours, value);
    }
    
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    public bool IsExportSuccessful
    {
        get => _isExportSuccessful;
        private set => SetProperty(ref _isExportSuccessful, value);
    }

    public byte[]? ExportedFileContent
    {
        get => _exportedFileContent;
        private set => SetProperty(ref _exportedFileContent, value);
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
    
    public Task ImportTour()
    {
        return Task.CompletedTask;
    }
    
    public async Task ExportTour()
    {
        if (string.IsNullOrEmpty(SelectedTourId))
        {
            ErrorMessage = "Please select a tour to export.";
            return;
        }

        try
        {
            var tourIds = new List<string> { SelectedTourId };
            var result = await tourService.ExportTourAsync(tourIds, true, "xlsx");
            if (result.isSuccess && result.fileContent != null)
            {
                ErrorMessage = null;
                ExportedFileContent = result.fileContent;
                IsExportSuccessful = true;
            }
            else
            {
                ErrorMessage = result.errorMessage;
                IsExportSuccessful = false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
            IsExportSuccessful = false;
        }
    }

    public void DownloadExportedTour()
    {
        if (ExportedFileContent == null) return;
        var base64 = Convert.ToBase64String(ExportedFileContent);
        var downloadLink = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}";
        navigationManager.NavigateTo(downloadLink, true);
    }
}
