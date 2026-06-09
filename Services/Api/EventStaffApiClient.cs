using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class EventStaffApiClient
{
    private readonly HttpClient _http;

    public EventStaffApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<PaginateEventStaffDto> GetAllAsync(
        string eventId,
        int page = 1, int limit = 10,
        string? name = null,
        string? email = null,
        string? invitationStatus = null,
        string? accessLevel = null)
    {
        Debug.WriteLine($"Fetching staff for event {eventId} with filters: name={name}, email={email}, invitationStatus={invitationStatus}, accessLevel={accessLevel}");
        var query = $"?page={page}&limit={limit}";
        if (!string.IsNullOrEmpty(name)) query += $"&name={Uri.EscapeDataString(name)}";
        if (!string.IsNullOrEmpty(email)) query += $"&email={Uri.EscapeDataString(email)}";
        if (!string.IsNullOrEmpty(invitationStatus)) query += $"&invitationStatus={Uri.EscapeDataString(invitationStatus)}";
        if (!string.IsNullOrEmpty(accessLevel)) query += $"&accessLevel={Uri.EscapeDataString(accessLevel)}";

        var response = await _http.GetAsync($"/api/events/{eventId}/staff{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PaginateEventStaffDto>(body, JsonConfig.Options)!;
    }

    public async Task<EventStaffResponseDto> GetByIdAsync(string staffId)
    {
        var response = await _http.GetAsync($"/api/events/staff/{staffId}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventStaffResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task DeleteAsync(string staffId)
    {
        var response = await _http.DeleteAsync($"/api/events/staff/{staffId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<EventStaffResponseDto> UpdateAccessLevelAsync(string staffId, UpdateEventStaffAccessLevelDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/events/{staffId}/access-level")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventStaffResponseDto>(body, JsonConfig.Options)!;
    }
}
