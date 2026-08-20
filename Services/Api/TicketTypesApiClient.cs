using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class TicketTypesApiClient
{
    private readonly HttpClient _http;

    public TicketTypesApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<TicketTypeListPageDto> GetAllAsync(
        string eventId,
        int page = 1, int limit = 10,
        string? name = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        bool availableOnly = false)
    {
        var query = $"?page={page}&limit={limit}&availableOnly={availableOnly.ToString().ToLower()}";
        if (!string.IsNullOrEmpty(name)) query += $"&name={Uri.EscapeDataString(name)}";
        if (startDate.HasValue) query += $"&startDate={startDate.Value:yyyy-MM-ddTHH:mm:ss.fffZ}";
        if (endDate.HasValue) query += $"&endDate={endDate.Value:yyyy-MM-ddTHH:mm:ss.fffZ}";

        var response = await _http.GetAsync($"/api/events/{eventId}/ticket-types{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TicketTypeListPageDto>(body, JsonConfig.Options)!;
    }

    public async Task<TicketTypeResponseDto> CreateAsync(string eventId, CreateTicketTypeDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"/api/events/{eventId}/ticket-types", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TicketTypeResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<TicketTypeResponseDto> UpdateAsync(string ticketTypeId, UpdateTicketTypeDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/events/ticket-types/{ticketTypeId}")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TicketTypeResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task DeleteAsync(string ticketTypeId)
    {
        var response = await _http.DeleteAsync($"/api/events/ticket-types/{ticketTypeId}");
        response.EnsureSuccessStatusCode();
    }
}
