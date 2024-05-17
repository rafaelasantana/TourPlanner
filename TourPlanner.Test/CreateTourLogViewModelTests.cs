using Microsoft.AspNetCore.Components;
using Moq;
using TourPlanner.Models;
using TourPlanner.Models.ResponseModels;
using TourPlanner.Models.TourLogModels;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;
using TourPlanner.ViewModels.TourLogViewModels;

namespace TourPlanner.Test;

public class CreateTourLogViewModelTests
{
    private readonly Mock<IHttpClientWrapper> _mockHttpClientWrapper;
    private readonly CreateTourLogViewModel _viewModel;

    public CreateTourLogViewModelTests()
    {
        _mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
        Mock<NavigationManager> mockNavigationManager = new();
        var tourLogService = new TourLogService(_mockHttpClientWrapper.Object);
        var tourService = new TourService(_mockHttpClientWrapper.Object);
        _viewModel = new CreateTourLogViewModel(tourLogService, tourService, mockNavigationManager.Object);
    }

    [Fact]
    public void Comment_SetValue_SetsComment()
    {
        // Arrange
        var comment = "Test comment";

        // Act
        _viewModel.Comment = comment;

        // Assert
        Assert.Equal(comment, _viewModel.Comment);
    }

    [Fact]
    public void UpdateTotalTime_SetsTotalTime()
    {
        // Act
        _viewModel.Hours = 1;
        _viewModel.Minutes = 30;

        // Assert
        Assert.Equal("01:30:00", _viewModel.TotalTime);
    }

    [Fact]
    public async Task CreateTourLog_Unsuccessful_SetsErrorMessage()
    {
        // Arrange
        var errorMessage = "Error creating tour log";
        _viewModel.Comment = "Test comment";
        _viewModel.Difficulty = DifficultyModel.Normal;
        _viewModel.TotalDistanceMeters = 100.5;
        _viewModel.Hours = 1;
        _viewModel.Minutes = 30;
        _viewModel.Rating = 5;
        _viewModel.SelectedTourId = "123";

        _mockHttpClientWrapper.Setup(wrapper => wrapper.PostAsJsonAsync($"tour-logs/123", It.IsAny<TourLogDTOModel>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new ApiErrorResponse
                {
                    Error = new ApiError { Message = errorMessage }
                }))
            });

        // Act
        var result = await _viewModel.CreateTourLog();

        // Assert
        Assert.Equal(errorMessage, _viewModel.ErrorMessage);
        Assert.Equal(errorMessage, result);
    }
}