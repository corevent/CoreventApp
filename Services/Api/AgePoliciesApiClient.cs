using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class AgePoliciesApiClient
{
    private readonly HttpClient _http;

    public AgePoliciesApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<AgePolicyResponseDto> GetActivePolicyAsync()
    {
        var response = await _http.GetAsync("/api/age-policies");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AgePolicyResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<CheckAcceptanceResponseDto> CheckAcceptanceAsync()
    {
        var response = await _http.GetAsync("/api/age-policies/acceptances/check");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CheckAcceptanceResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<AgePolicyAcceptanceResponseDto> AcceptPolicyAsync()
    {
        var response = await _http.PostAsync("/api/age-policies/acceptances", null);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AgePolicyAcceptanceResponseDto>(body, JsonConfig.Options)!;
    }
}
