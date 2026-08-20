using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class EventRatingsApiClient
{
    private readonly HttpClient _http;

    public EventRatingsApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<EventRatingResponseDto> CreateAsync(string eventId, CreateEventRatingDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"/api/events/{eventId}/ratings", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventRatingResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task UpdateAsync(string ratingId, CreateEventRatingDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _http.PatchAsync($"/api/events/ratings/{ratingId}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string ratingId)
    {
        var response = await _http.DeleteAsync($"/api/events/ratings/{ratingId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<MyRatingsListPageDto> GetMyRatingsAsync(int page = 1, int limit = 20)
    {
        var response = await _http.GetAsync($"/api/events/my/ratings?page={page}&limit={limit}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MyRatingsListPageDto>(body, JsonConfig.Options)!;
    }
}
