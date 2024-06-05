using System.Text.Json;
using TourPlanner.Models.ResponseModels;
using TourPlanner.Models.TourModels;

namespace TourPlanner.Services;

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
                return null;
            }
            catch (Exception)
            {
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
                    return (true, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                return (false, errorData?.Error?.Message ?? "An error occurred during update.");
            }
            catch (Exception ex)
            {
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
        
        public async Task<(bool isSuccess, byte[]? fileContent, string? errorMessage)> ExportTourAsync(List<string> tourIds, bool withTourLogs, string format)
        {
            if (tourIds == null || !tourIds.Any())
            {
                throw new ArgumentException("tourIds cannot be null or empty.", nameof(tourIds));
            }

            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format is required.", nameof(format));
            }

            try
            {
                var requestUri = $"tours/export?withTourLogs={withTourLogs}&format={format}";
                var response = await httpClientWrapper.PostAsJsonAsync(requestUri, tourIds);

                if (response.IsSuccessStatusCode)
                {
                    var fileContent = await response.Content.ReadAsByteArrayAsync();
                    return (true, fileContent, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                return (false, null, errorData?.Error?.Message ?? "An error occurred during export.");
            }
            catch (Exception ex)
            {
                return (false, null, $"Exception when calling API: {ex.Message}");
            }
        }
        
        public async Task<(bool isSuccess, string? errorMessage)> ImportTourAsync(Stream fileStream, string format)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                content.Add(fileContent, "SubmittedFile", "import.xlsx");

                var requestUri = $"tours/import?format={format}";
                var response = await httpClientWrapper.PostAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                    return (false, errorData?.Error?.Message ?? "An error occurred during import.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Exception when calling API: {ex.Message}");
            }
        }
        
        public async Task<(List<TourModel>? tours, string? errorMessage)> SearchToursAsync(string searchQuery)
        {
            try
            {
                var url = $"tours?$search={Uri.EscapeDataString(searchQuery)}";
                var response = await httpClientWrapper.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var toursResponse = JsonSerializer.Deserialize<TourListResponseModel>(responseBody);
                    return (toursResponse?.Tours, null);
                }
                else
                {
                    var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseBody);
                    var errorMessage = errorData?.Error?.Message ?? "An error occurred while fetching the tours.";
                    return (null, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (null, $"Exception when fetching tours: {ex.Message}");
            }
        }
        
        public async Task<List<TourModel>?> GetTopPopularToursAsync(int topN = 3)
        {
            try
            {
                var response = await httpClientWrapper.GetAsync($"tours?$orderby=popularity desc&$top={topN}");

                if (!response.IsSuccessStatusCode) return null;
                var tourResponse = await response.Content.ReadFromJsonAsync<TourListResponseModel>();
                return tourResponse?.Tours;
            }
            catch (Exception)
            {
                return null;
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