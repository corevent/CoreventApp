using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class FavoritesApiClient
{
    private readonly HttpClient _http;

    public FavoritesApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<FavoriteResponseDto> CreateAsync(string eventId)
    {
        var response = await _http.PostAsync($"/api/favorites/events/{eventId}", null);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FavoriteResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task DeleteAsync(string favoriteId)
    {
        var response = await _http.DeleteAsync($"/api/favorites/{favoriteId}");
        response.EnsureSuccessStatusCode();
    }
}
