using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class StaffInvitesApiClient
{
    private readonly HttpClient _http;

    public StaffInvitesApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<EventStaffInvitationResponseDto> CreateAsync(string eventId, CreateEventStaffInvitationDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"/api/invitations/events/{eventId}", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventStaffInvitationResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<PaginateEventStaffInvitationsDto> GetAllAsync(
        string eventId,
        string invitationStatus,
        int page = 1,
        int limit = 10
    )
    {
        Debug.WriteLine($"Fetching invitations for event {eventId} with filters: invitationStatus={invitationStatus}");
        var query = $"?page={page}&limit={limit}";
        if (!string.IsNullOrEmpty(invitationStatus)) query += $"&invitationStatus={Uri.EscapeDataString(invitationStatus)}";

        var response = await _http.GetAsync($"/api/invitations/events/{eventId}{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"Received response: {body}");
        return JsonSerializer.Deserialize<PaginateEventStaffInvitationsDto>(body, JsonConfig.Options)!;
    }

    public async Task<EventStaffResponseDto> AcceptAsync(string invitationId)
    {
        var response = await _http.PostAsync($"/api/invitations/{invitationId}/accept", null);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventStaffResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<MessageDto> RejectAsync(string invitationId)
    {
        var response = await _http.PostAsync($"/api/invitations/{invitationId}/reject", null);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MessageDto>(body, JsonConfig.Options)!;
    }

    public async Task<MessageDto> CancelAsync(string invitationId)
    {
        var response = await _http.PostAsync($"/api/invitations/{invitationId}/cancel", null);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MessageDto>(body, JsonConfig.Options)!;
    }

    public async Task<PaginateEventStaffInvitationsDto> GetMyInvitationsAsync(
        int page = 1, int limit = 10,
        string? name = null,
        string? email = null,
        string? invitationStatus = null,
        string? originalAccessLevel = null)
    {
        var query = $"?page={page}&limit={limit}";
        if (!string.IsNullOrEmpty(name)) query += $"&name={Uri.EscapeDataString(name)}";
        if (!string.IsNullOrEmpty(email)) query += $"&email={Uri.EscapeDataString(email)}";
        if (!string.IsNullOrEmpty(invitationStatus)) query += $"&invitationStatus={Uri.EscapeDataString(invitationStatus)}";
        if (!string.IsNullOrEmpty(originalAccessLevel)) query += $"&originalAccessLevel={Uri.EscapeDataString(originalAccessLevel)}";

        var response = await _http.GetAsync($"/api/invitations/me{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PaginateEventStaffInvitationsDto>(body, JsonConfig.Options)!;
    }
}
