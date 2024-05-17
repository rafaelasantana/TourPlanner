using System.Text.Json;
using TourPlanner.Models.ResponseModels;
using TourPlanner.Models.TourLogModels;

namespace TourPlanner.Services;

using System.Threading.Tasks;
using Models;

public class TourLogService(IHttpClientWrapper httpClientWrapper)
{
    
    public async Task<(List<TourLogModel>? logs, string? errorMessage)> GetTourLogsAsync(string tourId)
    {
        try
        {
            var url = $"tour-logs?$filter=TourId eq {tourId}";
            var response = await httpClientWrapper.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var logsResponse = JsonSerializer.Deserialize<TourLogListResponseModel>(responseBody);
                return (logsResponse?.TourLogs, null);
            }
            else
            {
                var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseBody);
                var errorMessage = errorData?.Error?.Message ?? "An error occurred while fetching the tour logs.";
                return (null, errorMessage);
            }
        }
        catch (Exception ex)
        {
            return (null, $"Exception when fetching tour logs: {ex.Message}");
        }
    }

    public async Task<(bool isSuccess, string? errorMessage)> CreateTourLogAsync(TourLogDTOModel tourLogDto, string tourId)
    {
        try
        {
            var response = await httpClientWrapper.PostAsJsonAsync($"tour-logs/{tourId}", tourLogDto);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent);
            string errorMessage = errorData?.Error?.Message ?? "An error occurred while creating the tour log.";
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            return (false, $"Exception when creating tour log: {ex.Message}");
        }
    }

    public async Task<(TourLogModel? tourLog, string? errorMessage)> GetTourLogByIdAsync(string tourLogId)
    {
        try
        {
            var response = await httpClientWrapper.GetAsync($"tour-logs/{tourLogId}");
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tourLog = JsonSerializer.Deserialize<TourLogModel>(responseBody);
                return (tourLog, null);
            }

            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseBody);
            return (null, errorData?.Error?.Message ?? "An error occurred while fetching the tour log.");
        }
        catch (Exception ex)
        {
            return (null, $"Exception when fetching tour log by ID: {ex.Message}");
        }
    }

    public async Task<(bool isSuccess, string? errorMessage)> UpdateTourLogAsync(TourLogDTOModel tourLogDto, string tourLogId)
    {
        try
        {
            var response = await httpClientWrapper.PutAsJsonAsync($"tour-logs/{tourLogId}", tourLogDto);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent);
            string errorMessage = errorData?.Error?.Message ?? "An error occurred while updating the tour log.";
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            return (false, $"Exception when updating tour log: {ex.Message}");
        }
    }

    public async Task<(bool isSuccess, string? errorMessage)> DeleteTourLogAsync(string tourLogId)
    {
        try
        {
            var response = await httpClientWrapper.DeleteAsync($"tour-logs/{tourLogId}");

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
            return (false, errorData?.Error?.Message ?? "An error occurred during deletion.");
        }
        catch (Exception ex)
        {
            return (false, $"Exception when deleting tour log: {ex.Message}");
        }
    }
}
