using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class TicketsApiClient
{
    private readonly HttpClient _http;

    public TicketsApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<MyTicketsResponseDto> GetMyTicketsByEventAsync(string eventId)
    {
        var response = await _http.GetAsync($"/api/events/{eventId}/my/tickets");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MyTicketsResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<PaginateMyTicketsDto> GetMyTicketsAsync(int page = 1, int limit = 100, string? eventId = null)
    {
        var query = $"?page={page}&limit={limit}";
        if (!string.IsNullOrEmpty(eventId))
            query += $"&eventId={Uri.EscapeDataString(eventId)}";

        var response = await _http.GetAsync($"/api/users/me/tickets{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PaginateMyTicketsDto>(body, JsonConfig.Options)!;
    }
}
