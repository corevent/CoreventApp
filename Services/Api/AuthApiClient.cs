using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class AuthApiClient
{
    private readonly HttpClient _http;

    public AuthApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<AuthTokensDto> Login(LoginDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthTokensDto>(body, JsonConfig.Options)!;
    }

    public async Task<AuthTokensDto> Refresh(RefreshTokenDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/auth/refresh", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthTokensDto>(body, JsonConfig.Options)!;
    }

    public async Task Logout(RefreshTokenDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/auth/logout", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<MessageDto> ForgotPassword(EmailDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/auth/forgot-password", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MessageDto>(body, JsonConfig.Options)!;
    }

    public async Task<MessageDto> ResetPassword(ResetPasswordDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/auth/reset-password", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MessageDto>(body, JsonConfig.Options)!;
    }

    public async Task<MessageDto> VerifyEmail(EmailDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/auth/verify-email", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MessageDto>(body, JsonConfig.Options)!;
    }
}
