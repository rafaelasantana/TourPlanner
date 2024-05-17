using System.Text.Json;
using TourPlanner.Models;
using TourPlanner.Models.ResponseModels;
using TourPlanner.Models.TourLogModels;

namespace TourPlanner.Services;

public class TourLogService(HttpClient httpClient)
{
    public async Task<(List<TourLogModel>? logs, string? errorMessage)> GetTourLogsAsync(string tourId)
    {
        Console.WriteLine("In tour log service: GetTourLogsAsync");
        try
        {
            // Log the URL that will be requested
            var url = $"tour-logs?$filter=TourId eq {tourId}";
            Console.WriteLine($"Requesting URL: {url}");

            var response = await httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync(); // Read the raw response body

            // Log the raw response body
            Console.WriteLine($"Response Status: {response.StatusCode}");
            Console.WriteLine($"Raw Response Body: {responseBody}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Response indicates success. Processing data...");

                var logsResponse = JsonSerializer.Deserialize<TourLogListResponseModel>(responseBody);
                if (logsResponse?.TourLogs != null)
                {
                    Console.WriteLine($"Deserialized Tour Logs Count: {logsResponse.TourLogs.Count}"); // Log the count of tour logs
                }
                return (logsResponse?.TourLogs, null);
            }
            else
            {
                Console.WriteLine("Response indicates failure. Attempting to parse error message...");

                var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseBody);
                var errorMessage = errorData?.Error?.Message ?? "An error occurred while fetching the tour logs.";
                Console.WriteLine($"Error Message: {errorMessage}");
                return (null, errorMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when fetching tour logs: {ex.Message}");
            return (null, $"Exception when fetching tour logs: {ex.Message}");
        }
    }

    
    public async Task<(bool isSuccess, string? errorMessage)> CreateTourLogAsync(TourLogDTOModel tourLogDto, string tourId)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"tour-logs/{tourId}", tourLogDto);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully created tour log!");
                return (true, null);
            }

            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent);
            string errorMessage = errorData?.Error?.Message ?? "An error occurred while creating the tour log.";
            Console.WriteLine($"Error in creating tour log: {errorMessage}");
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when creating tour log: {ex.Message}");
            return (false, $"Exception when creating tour log: {ex.Message}");
        }
    }
    
    public async Task<(TourLogModel? tourLog, string? errorMessage)> GetTourLogByIdAsync(string tourLogId)
    {
        Console.WriteLine("In tour log service, try to get tour log by id");
        try
        {
            var response = await httpClient.GetAsync($"tour-logs/{tourLogId}");
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("found tour log");
                var tourLog = JsonSerializer.Deserialize<TourLogModel>(responseBody);
                return (tourLog, null);
            }
            Console.WriteLine("could not find tour log");
            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseBody);
            return (null, errorData?.Error?.Message ?? "An error occurred while fetching the tour log.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when fetching tour log by ID: {ex.Message}");
            return (null, $"Exception when fetching tour log by ID: {ex.Message}");
        }
    }
    
    public async Task<(bool isSuccess, string? errorMessage)> UpdateTourLogAsync(TourLogDTOModel tourLogDto, string tourLogId)
    {
        Console.WriteLine("In tour log service: UpdateTourLogAsync");
    
        try
        {
            // Serialize the DTO to JSON and log it for debugging
            var requestJson = JsonSerializer.Serialize(tourLogDto);
            Console.WriteLine($"Request JSON: {requestJson}");

            var response = await httpClient.PutAsJsonAsync($"tour-logs/{tourLogId}", tourLogDto);
            var responseContent = await response.Content.ReadAsStringAsync();
        
            // Log the status code and response for better clarity on the result
            Console.WriteLine($"Response Status: {response.StatusCode}");
            Console.WriteLine($"Response Content: {responseContent}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully updated tour log!");
                return (true, null);
            }

            Console.WriteLine("Update failed!");
            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent);
            string errorMessage = errorData?.Error?.Message ?? "An error occurred while updating the tour log.";
            Console.WriteLine($"Error in updating tour log: {errorMessage}");
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when updating tour log: {ex.Message}");
            return (false, $"Exception when updating tour log: {ex.Message}");
        }
    }

    
    public async Task<(bool isSuccess, string? errorMessage)> DeleteTourLogAsync(string tourLogId)
    {
        Console.WriteLine("In tour log service DeleteTourLogAsync");

        try
        {
            var response = await httpClient.DeleteAsync($"tour-logs/{tourLogId}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Tour log deleted successfully.");
                return (true, null);
            }
            Console.WriteLine("DeleteTourLogAsync failed");

            var errorContent = await response.Content.ReadAsStringAsync();
            var errorData = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
            return (false, errorData?.Error?.Message ?? "An error occurred during deletion.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when deleting tour log: {ex.Message}");
            return (false, $"Exception when deleting tour log: {ex.Message}");
        }
    }


}