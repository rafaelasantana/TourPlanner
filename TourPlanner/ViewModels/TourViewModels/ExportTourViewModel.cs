using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class ExportTourViewModel(TourService tourService, NavigationManager navigationManager)
    : ObservableObject
{
    private string _selectedTourId = string.Empty;
    private List<TourModel> _tours = new();
    private string? _errorMessage;
    private string _format = "xlsx";
    private bool _isExportSuccessful;
    public bool IncludeTourLogs = true;
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
    
    public string Format
    {
        get => _format;
        set => SetProperty(ref _format, value);
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
            var result = await tourService.ExportTourAsync(tourIds, IncludeTourLogs, _format);
            if (result.isSuccess && result.fileContent != null)
            {
                ErrorMessage = null;
                ExportedFileContent = result.fileContent;
                IsExportSuccessful = true;

                DownloadExportedTour();
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
        var mimeType = Format.ToLower() == "csv" ? "text/csv" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var downloadLink = $"data:{mimeType};base64,{base64}";
        navigationManager.NavigateTo(downloadLink, true);
    }
}
