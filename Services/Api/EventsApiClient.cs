using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class EventsApiClient
{
    private readonly HttpClient _http;

    public EventsApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<EventResponseDto> CreateAsync(CreateEventDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/events", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<EventListPageDto> GetAllAsync(
        int page = 1, int limit = 10,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        string? status = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        var query = $"?page={page}&limit={limit}";
        if (!string.IsNullOrEmpty(search)) query += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrEmpty(category)) query += $"&category={Uri.EscapeDataString(category)}";
        if (startDate.HasValue) query += $"&startDate={startDate.Value:yyyy-MM-dd}";
        if (!string.IsNullOrEmpty(status)) query += $"&status={Uri.EscapeDataString(status)}";
        if (isAdultOnly.HasValue) query += $"&isAdultOnly={isAdultOnly.Value.ToString().ToLower()}";
        if (stateId.HasValue) query += $"&stateId={stateId.Value}";
        if (cityId.HasValue) query += $"&cityId={cityId.Value}";

        var response = await _http.GetAsync($"/api/events{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventListPageDto>(body, JsonConfig.Options)!;
    }

    public async Task<EventResponseDto> GetByIdAsync(string id)
    {
        var response = await _http.GetAsync($"/api/events/{id}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<EventResponseDto> UpdateAsync(string id, UpdateEventDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/events/{id}")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<EventListPageDto> GetMyOrganizerEventsAsync(
        int page, int limit,
        string status,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        var query = $"?page={page}&limit={limit}&status={Uri.EscapeDataString(status)}";
        if (!string.IsNullOrEmpty(search)) query += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrEmpty(category)) query += $"&category={Uri.EscapeDataString(category)}";
        if (startDate.HasValue) query += $"&startDate={startDate.Value:yyyy-MM-dd}";
        if (isAdultOnly.HasValue) query += $"&isAdultOnly={isAdultOnly.Value.ToString().ToLower()}";
        if (stateId.HasValue) query += $"&stateId={stateId.Value}";
        if (cityId.HasValue) query += $"&cityId={cityId.Value}";

        var response = await _http.GetAsync($"/api/events/my/organizer{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventListPageDto>(body, JsonConfig.Options)!;
    }

    public async Task<StaffEventListPageDto> GetMyStaffEventsAsync(
        int page, int limit,
        string status,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        var query = $"?page={page}&limit={limit}&status={Uri.EscapeDataString(status)}";
        if (!string.IsNullOrEmpty(search)) query += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrEmpty(category)) query += $"&category={Uri.EscapeDataString(category)}";
        if (startDate.HasValue) query += $"&startDate={startDate.Value:yyyy-MM-dd}";
        if (isAdultOnly.HasValue) query += $"&isAdultOnly={isAdultOnly.Value.ToString().ToLower()}";
        if (stateId.HasValue) query += $"&stateId={stateId.Value}";
        if (cityId.HasValue) query += $"&cityId={cityId.Value}";

        var response = await _http.GetAsync($"/api/events/my/staff{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StaffEventListPageDto>(body, JsonConfig.Options)!;
    }

    public async Task DeleteAsync(string id)
    {
        var response = await _http.DeleteAsync($"/api/events/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task CancelAsync(string id)
    {
        var response = await _http.PostAsync($"/api/events/{id}/cancel", null);
        response.EnsureSuccessStatusCode();
    }
}
