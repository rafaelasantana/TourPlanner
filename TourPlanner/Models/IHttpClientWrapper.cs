using System.Net.Http.Json;

namespace TourPlanner.Models;

public interface IHttpClientWrapper
{
    Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value);
    Task<HttpResponseMessage> GetAsync(string requestUri);
    Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T value);
    Task<HttpResponseMessage> DeleteAsync(string requestUri);
}

public class HttpClientWrapper(HttpClient httpClient) : IHttpClientWrapper
{
    public Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value)
    {
        return httpClient.PostAsJsonAsync(requestUri, value);
    }

    public Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        return httpClient.GetAsync(requestUri);
    }

    public Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T value)
    {
        return httpClient.PutAsJsonAsync(requestUri, value);
    }

    public Task<HttpResponseMessage> DeleteAsync(string requestUri)
    {
        return httpClient.DeleteAsync(requestUri);
    }
}
