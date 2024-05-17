/*using System.Text.Json;
using TourPlanner.Models.ResponseModels;
using TourPlanner.Models.TourModels;
using TourPlanner.Models;

namespace TourPlanner.Services;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Models;

public class TourService(HttpClient httpClient)
{
    public async Task<(TourModel? tour, string? errorMessage)> CreateTourAsync(TourDTOModel tourDtoModel)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("tours", tourDtoModel);
            
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<TourModel>(), null);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
            return (null, errorData?.Error?.Message ?? "An error occurred, please try again.");
        }
        catch (Exception ex)
        {
            return (null, $"Exception when calling API: {ex.Message}");
        }
    }
    
    public async Task<List<TourModel>?> GetAllToursAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("tours");
            
            if (response.IsSuccessStatusCode)
            {
                var tourResponse = await response.Content.ReadFromJsonAsync<TourListResponseModel>();
                return tourResponse?.Tours;
            }

            Console.WriteLine($"Failed to fetch tours: {response.StatusCode}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when fetching tours: {ex.Message}");
            return null;
        }
    }
    
    public async Task<(TourModel? tour, string? errorMessage)> GetTourByIdAsync(string? tourId)
    {
        try
        {
            var response = await httpClient.GetAsync($"tours/{tourId}");

            if (response.IsSuccessStatusCode)
            {
                var tour = await response.Content.ReadFromJsonAsync<TourModel>();
                return (tour, null);
            }

            // If the status code is not successful, log the status and read the error message if available.
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
            return (null, errorData?.Error?.Message ?? "Could not retrieve this tour.");
        }
        catch (Exception ex)
        {
            // Handle unexpected exceptions, which could include network issues, serialization errors, etc.
            return (null, $"Exception when fetching tour: {ex.Message}");
        }
    }
    
    public async Task<(bool isSuccess, string? errorMessage)> UpdateTourAsync(TourModel tourModel)
    {
        try
        {
            // Serialize the tourModel to JSON for logging
            var requestBody = JsonSerializer.Serialize(tourModel);
            Console.WriteLine($"Request Body for update: {requestBody}");

            // Now send the actual request
            var response = await httpClient.PutAsJsonAsync($"tours/{tourModel.Id}", tourModel);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Tour updated successfully.");
                return (true, null);
            }

            Console.WriteLine("Failed to update tour - Status Code: " + response.StatusCode);
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Error content received: " + errorContent);

            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
            return (false, errorData?.Error?.Message ?? "An error occurred during update.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when updating tour: {ex.Message}");
            return (false, $"Exception when updating tour: {ex.Message}");
        }
    }
    
    public async Task<(bool isSuccess, string errorMessage)> DeleteTourAsync(string tourId)
    {
        Console.WriteLine($"In tour service: DeleteTourAsync, id= {tourId}");
        try
        {
            var response = await httpClient.DeleteAsync($"tours/{tourId}");
            if (response.IsSuccessStatusCode)
            {
                return (true, "Tour deleted successfully.");
            }
            else
            {
                Console.WriteLine("Delete failed");
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                return (false, errorData?.Error?.Message ?? "An error occurred during deletion.");
            }
        }
        catch (Exception ex)
        {
            return (false, $"Exception when deleting tour: {ex.Message}");
        }
    }
}
public class ApiErrorResponse
{
    public ApiError? Error { get; set; }
}

public class ApiError
{
    public string? Code { get; set; }
    public string? Message { get; set; }
}*/

using System.Text.Json;
using TourPlanner.Models.ResponseModels;
using TourPlanner.Models.TourModels;
using TourPlanner.Models;

namespace TourPlanner.Services;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Models;

public class TourService(IHttpClientWrapper httpClientWrapper)
{

        public async Task<(TourModel? tour, string? errorMessage)> CreateTourAsync(TourDTOModel tourDtoModel)
        {
            try
            {
                var response = await httpClientWrapper.PostAsJsonAsync("tours", tourDtoModel);

                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadFromJsonAsync<TourModel>(), null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                return (null, errorData?.Error?.Message ?? "An error occurred, please try again.");
            }
            catch (Exception ex)
            {
                return (null, $"Exception when calling API: {ex.Message}");
            }
        }

        public async Task<List<TourModel>?> GetAllToursAsync()
        {
            try
            {
                var response = await httpClientWrapper.GetAsync("tours");

                if (response.IsSuccessStatusCode)
                {
                    var tourResponse = await response.Content.ReadFromJsonAsync<TourListResponseModel>();
                    return tourResponse?.Tours;
                }

                Console.WriteLine($"Failed to fetch tours: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when fetching tours: {ex.Message}");
                return null;
            }
        }

        public async Task<(TourModel? tour, string? errorMessage)> GetTourByIdAsync(string? tourId)
        {
            try
            {
                var response = await httpClientWrapper.GetAsync($"tours/{tourId}");

                if (response.IsSuccessStatusCode)
                {
                    var tour = await response.Content.ReadFromJsonAsync<TourModel>();
                    return (tour, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                return (null, errorData?.Error?.Message ?? "Could not retrieve this tour.");
            }
            catch (Exception ex)
            {
                return (null, $"Exception when fetching tour: {ex.Message}");
            }
        }

        public async Task<(bool isSuccess, string? errorMessage)> UpdateTourAsync(TourModel tourModel)
        {
            try
            {
                var response = await httpClientWrapper.PutAsJsonAsync($"tours/{tourModel.Id}", tourModel);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Tour updated successfully.");
                    return (true, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                return (false, errorData?.Error?.Message ?? "An error occurred during update.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when updating tour: {ex.Message}");
                return (false, $"Exception when updating tour: {ex.Message}");
            }
        }

        public async Task<(bool isSuccess, string errorMessage)> DeleteTourAsync(string tourId)
        {
            try
            {
                var response = await httpClientWrapper.DeleteAsync($"tours/{tourId}");
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Tour deleted successfully.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                    return (false, errorData?.Error?.Message ?? "An error occurred during deletion.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Exception when deleting tour: {ex.Message}");
            }
        }
}
public class ApiErrorResponse
{
    public ApiError? Error { get; set; }
}

public class ApiError
{
    public string? Code { get; set; }
    public string? Message { get; set; }
}