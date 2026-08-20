using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class StatesApiClient
{
    private readonly HttpClient _http;

    public StatesApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<StateResponseDto> GetStatesAsync()
    {
        var response = await _http.GetAsync("/api/states");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StateResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<CityResponseDto> GetCitiesAsync(int stateId)
    {
        var response = await _http.GetAsync($"/api/states/{stateId}/cities");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CityResponseDto>(body, JsonConfig.Options)!;
    }
}
