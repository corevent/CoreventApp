using System.Net.Http.Headers;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class StorageApiClient
{
    private readonly HttpClient _http;

    public StorageApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<PresignUploadResponseDto> PresignUploadAsync(PresignUploadDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/api/storage/presign", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PresignUploadResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<bool> UploadImageAsync(string uploadUrl, Stream imageStream, string contentType)
    {
        try
        {
            using var uploadClient = new HttpClient();
            using var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            var response = await uploadClient.PutAsync(uploadUrl, streamContent);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task ConfirmAvatarUploadAsync(string key)
    {
        var json = JsonSerializer.Serialize(new ConfirmImageUploadDto(key), JsonConfig.Options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/users/me/avatar")
        {
            Content = content
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task ConfirmEventBannerAsync(string eventId, string key)
    {
        var json = JsonSerializer.Serialize(new ConfirmImageUploadDto(key), JsonConfig.Options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/events/{eventId}/banner")
        {
            Content = content
        };
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
