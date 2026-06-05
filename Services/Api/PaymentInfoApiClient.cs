using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class PaymentInfoApiClient
{
    private readonly HttpClient _http;

    public PaymentInfoApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<OrganizerPaymentInfoResDto> CreateAsync(CreateOrganizerPaymentInfoDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/users/me/organizer-payment-info", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrganizerPaymentInfoResDto>(body, JsonConfig.Options)!;
    }

    public async Task<OrganizerPaymentInfoPageDto> GetAllAsync(int page = 1, int limit = 10)
    {
        var response = await _http.GetAsync($"/api/users/me/organizer-payment-info?page={page}&limit={limit}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrganizerPaymentInfoPageDto>(body, JsonConfig.Options)!;
    }

    public async Task<OrganizerPaymentInfoResDto> GetByIdAsync(string id)
    {
        var response = await _http.GetAsync($"/api/users/me/organizer-payment-info/{id}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrganizerPaymentInfoResDto>(body, JsonConfig.Options)!;
    }

    public async Task<OrganizerPaymentInfoResDto> UpdateAsync(string id, UpdateOrganizerPaymentInfoDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/users/me/organizer-payment-info/{id}")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrganizerPaymentInfoResDto>(body, JsonConfig.Options)!;
    }

    public async Task DeleteAsync(string id)
    {
        var response = await _http.DeleteAsync($"/api/users/me/organizer-payment-info/{id}");
        response.EnsureSuccessStatusCode();
    }
}
