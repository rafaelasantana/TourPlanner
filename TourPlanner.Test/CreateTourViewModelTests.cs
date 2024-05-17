using Microsoft.AspNetCore.Components;
using Moq;
using TourPlanner.Models;
using TourPlanner.Models.TourModels;
using TourPlanner.Services;
using TourPlanner.ViewModels.TourViewModels;

namespace TourPlanner.Test;

public class CreateTourViewModelTests
{
    private readonly Mock<IHttpClientWrapper> _mockHttpClientWrapper;
    private readonly CreateTourViewModel _viewModel;

    public CreateTourViewModelTests()
    {
        _mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
        Mock<NavigationManager> mockNavigationManager = new();
        var tourService = new TourService(_mockHttpClientWrapper.Object);
        _viewModel = new CreateTourViewModel(tourService, mockNavigationManager.Object);
    }

    [Fact]
    public void Description_SetValue_SetsDescription()
    {
        // Arrange
        var description = "Test description";

        // Act
        _viewModel.Description = description;

        // Assert
        Assert.Equal(description, _viewModel.Description);
    }

    [Fact]
    public void Name_SetValue_SetsName()
    {
        // Arrange
        var name = "Test name";

        // Act
        _viewModel.Name = name;

        // Assert
        Assert.Equal(name, _viewModel.Name);
    }

    [Fact]
    public async Task HandleValidSubmit_InvalidTour_SetsErrorMessage()
    {
        // Arrange
        _viewModel.Description = "Test description";
        _viewModel.Name = "Test name";
        _viewModel.TransportType = TransportTypeModel.Car;
        _viewModel.Start = "Test start";
        _viewModel.End = "Test end";
        var errorMessage = "Error creating tour";

        _mockHttpClientWrapper.Setup(client => client.PostAsJsonAsync("tours", It.IsAny<TourDTOModel>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new ApiErrorResponse
                {
                    Error = new ApiError { Message = errorMessage }
                }))
            });

        // Act
        await _viewModel.HandleValidSubmit();

        // Assert
        Assert.Equal(errorMessage, _viewModel.ErrorMessage);
    }
}