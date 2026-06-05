using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class AttractionsApiClient
{
    private readonly HttpClient _http;

    public AttractionsApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<AttractionResponseDto> CreateAsync(string eventId, CreateAttractionDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"/api/events/{eventId}/attractions", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AttractionResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<AttractionListPageDto> GetAllAsync(
        string eventId,
        int page = 1, int limit = 10,
        string? search = null,
        string? guest = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var query = $"?page={page}&limit={limit}";
        if (!string.IsNullOrEmpty(search)) query += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrEmpty(guest)) query += $"&guest={Uri.EscapeDataString(guest)}";
        if (startDate.HasValue) query += $"&startDate={startDate.Value:yyyy-MM-dd}";
        if (endDate.HasValue) query += $"&endDate={endDate.Value:yyyy-MM-dd}";

        var response = await _http.GetAsync($"/api/events/{eventId}/attractions{query}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AttractionListPageDto>(body, JsonConfig.Options)!;
    }

    public async Task<AttractionResponseDto> GetByIdAsync(string attractionId)
    {
        var response = await _http.GetAsync($"/api/events/attractions/{attractionId}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AttractionResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<AttractionResponseDto> UpdateAsync(string attractionId, UpdateAttractionDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/events/attractions/{attractionId}")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AttractionResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task DeleteAsync(string attractionId)
    {
        var response = await _http.DeleteAsync($"/api/events/attractions/{attractionId}");
        response.EnsureSuccessStatusCode();
    }
}
