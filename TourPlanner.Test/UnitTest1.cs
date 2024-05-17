using Xunit;  
using Moq;  
using TourPlanner.Services;  
using TourPlanner.ViewModels.TourViewModels;  
using System.Threading.Tasks;  
using TourPlanner.Models.TourModels;  
using TourPlanner.Models;  
using System.Net.Http;  
using System.Net;  
using System.Net.Http.Json;  

public class CreateTourViewModelTests  
{  
    private readonly Mock<IHttpClientWrapper> _mockHttpClientWrapper;  
    private readonly CreateTourViewModel _viewModel;  
  
    public CreateTourViewModelTests()  
    {        _mockHttpClientWrapper = new Mock<IHttpClientWrapper>();  
        var tourService = new TourService(_mockHttpClientWrapper.Object);  
        _viewModel = new CreateTourViewModel(tourService, null!);  
    }  
    [Fact]  
    public void Description_SetValue_SetsDescription()  
    {        // Arrange  
        var description = "Test description";  
  
        // Act  
        _viewModel.Description = description;  
  
        // Assert  
        Assert.Equal(description, _viewModel.Description);  
    }  
    [Fact]  
    public void Name_SetValue_SetsName()  
    {        // Arrange  
        var name = "Test name";  
  
        // Act  
        _viewModel.Name = name;  
  
        // Assert  
        Assert.Equal(name, _viewModel.Name);  
    }  
    /*[Fact]  
    public async Task HandleValidSubmit_ValidTour_CreatesTour()    {        // Arrange        _viewModel.Description = "Test description";        _viewModel.Name = "Test name";        _viewModel.TransportType = TransportTypeModel.Car;        _viewModel.Start = "Test start";        _viewModel.End = "Test end";  
        var generatedId = "generated-id"; // Simulated generated ID by the backend        var createdTour = new TourModel { Id = generatedId };  
        _mockHttpClientWrapper.Setup(client => client.PostAsJsonAsync("tours", It.IsAny<TourDTOModel>()))            .ReturnsAsync(new HttpResponseMessage            {                StatusCode = HttpStatusCode.OK,                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(createdTour))            });  
        // Act        await _viewModel.HandleValidSubmit();  
        // Assert        _mockHttpClientWrapper.Verify(client => client.PostAsJsonAsync("tours", It.IsAny<TourDTOModel>()), Times.Once);        Assert.Null(_viewModel.ErrorMessage);    }*/  
    [Fact]  
    public async Task HandleValidSubmit_InvalidTour_SetsErrorMessage()  
    {        // Arrange  
        _viewModel.Description = "Test description";  
        _viewModel.Name = "Test name";  
        _viewModel.TransportType = TransportTypeModel.Car;  
        _viewModel.Start = "Test start";  
        _viewModel.End = "Test end";  
        var errorMessage = "Error creating tour";  
  
        _mockHttpClientWrapper.Setup(client => client.PostAsJsonAsync("tours", It.IsAny<TourDTOModel>()))  
            .ReturnsAsync(new HttpResponseMessage  
            {  
                StatusCode = HttpStatusCode.BadRequest,  
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new ApiErrorResponse  
                {  
                    Error = new ApiError { Message = errorMessage }  
                }))            });  
        // Act  
        await _viewModel.HandleValidSubmit();  
  
        // Assert  
        Assert.Equal(errorMessage, _viewModel.ErrorMessage);  
    }}


