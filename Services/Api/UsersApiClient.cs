using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class UsersApiClient
{
    private readonly HttpClient _http;

    public UsersApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<UserResponseDto> CreateUser(CreateUserDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/users", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<UserResponseDto> UpdateUser(UpdateUserDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/users")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<MessageDto> UpdatePassword(UpdatePassDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/users/pass")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MessageDto>(body, JsonConfig.Options)!;
    }

    public async Task<UserResponseDto> GetProfile()
    {
        var response = await _http.GetAsync("/api/users/me");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserResponseDto>(body, JsonConfig.Options)!;
    }
}
