using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly TokenService _tokenService;
    private readonly AuthApiClient _authApi;

    public AuthTokenHandler(TokenService tokenService, AuthApiClient authApi)
    {
        _tokenService = tokenService;
        _authApi = authApi;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        if (string.IsNullOrEmpty(refreshToken))
            return response;

        try
        {
            var tokens = await _authApi.Refresh(new RefreshTokenDto(refreshToken));
            await _tokenService.SaveTokensAsync(tokens.AccessToken, tokens.RefreshToken);

            var retry = await CloneRequest(request);
            retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            return await base.SendAsync(retry, cancellationToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Token refresh failed: {ex.Message}");
            await _tokenService.ClearTokensAsync();
            return response;
        }
    }

    private static async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);
        if (request.Content != null)
        {
            var body = await request.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(body);
            if (request.Content.Headers.ContentType != null)
                clone.Content.Headers.ContentType = request.Content.Headers.ContentType;
        }
        foreach (var header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        return clone;
    }
}
