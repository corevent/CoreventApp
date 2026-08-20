using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class ParticipantsApiClient
{
    private readonly HttpClient _http;

    public ParticipantsApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<ParticipantListPageDto> GetAllAsync(string eventId, int page = 1, int limit = 100)
    {
        var body = await _http.GetStringAsync($"/api/events/{eventId}/participants?page={page}&limit={limit}");
        return JsonSerializer.Deserialize<ParticipantListPageDto>(body, JsonConfig.Options)!;
    }
}
