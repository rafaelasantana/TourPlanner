using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.TourViewModels;

public class ImportTourViewModel : ObservableObject
{
    private readonly TourService _tourService;
    private readonly NavigationManager _navigationManager;
    private string? _errorMessage;
    private string _format = "xlsx";
    private bool _isSuccess;
    private IBrowserFile? _importFile;

    public ImportTourViewModel(TourService tourService, NavigationManager navigationManager)
    {
        _tourService = tourService;
        _navigationManager = navigationManager;
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
    
    public bool IsSuccess
    {
        get => _isSuccess;
        private set => SetProperty(ref _isSuccess, value);
    }

    [Required(ErrorMessage = "Please select a file to import.")]
    public IBrowserFile? ImportFile
    {
        get => _importFile;
        set => SetProperty(ref _importFile, value);
    }

    public async Task ImportTour()
    {
        if (ImportFile == null)
        {
            ErrorMessage = "Please select a file to import.";
            return;
        }

        try
        {
            using var memoryStream = new MemoryStream();
            await ImportFile.OpenReadStream().CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset the position to the beginning of the stream
            var result = await _tourService.ImportTourAsync(memoryStream, _format);

            if (result.isSuccess)
            {
                ErrorMessage = null;
                IsSuccess = true;
            }
            else
            {
                ErrorMessage = result.errorMessage ?? "An error occurred during import.";
                IsSuccess = false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
            IsSuccess = false;
        }
    }
    
    public void ResetStatus()
    {
        IsSuccess = false;
    }
}
