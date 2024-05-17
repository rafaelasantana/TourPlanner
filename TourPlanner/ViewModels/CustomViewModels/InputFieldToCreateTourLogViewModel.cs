using Microsoft.AspNetCore.Components;
using TourPlanner.Services;

namespace TourPlanner.ViewModels.CustomViewModels;

public class InputFieldToCreateTourLogViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;

    public InputFieldToCreateTourLogViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    private string _tourId = string.Empty;
    private string _createTourLogComment = string.Empty;
    private string _errorMessage = string.Empty;

    public string TourId
    {
        get => _tourId;
        set => SetProperty(ref _tourId, value);
    }

    public string CreateTourLogComment
    {
        get => _createTourLogComment;
        set => SetProperty(ref _createTourLogComment, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public string CreateTourLogUrl => !string.IsNullOrEmpty(CreateTourLogComment)
        ? !string.IsNullOrEmpty(TourId)
            ? $"/tour-log/create?comment={Uri.EscapeDataString(CreateTourLogComment)}&tourId={Uri.EscapeDataString(TourId)}"
            : $"/tour-log/create?comment={Uri.EscapeDataString(CreateTourLogComment)}"
        : "/tour-log/create";

    public void HandleCreateTourLog()
    {
        if (!string.IsNullOrEmpty(CreateTourLogComment))
        {
            _navigationManager.NavigateTo(CreateTourLogUrl);
        }
        else
        {
            ErrorMessage = "Please ensure all fields are filled correctly.";
        }
    }
}