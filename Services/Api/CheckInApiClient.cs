using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class CheckInApiClient
{
    private readonly HttpClient _http;

    public CheckInApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<CheckinResponseDto> CheckinAsync(string eventId, CheckinDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"/api/events/{eventId}/checkin", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CheckinResponseDto>(body, JsonConfig.Options)!;
    }
}
