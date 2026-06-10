using System.Diagnostics;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class StorageService
{
    private readonly StorageApiClient _api;

    public StorageService(StorageApiClient api)
    {
        _api = api;
    }

    public async Task<string?> UploadAvatarAsync(Stream imageStream, string contentType)
    {
        try
        {
            var presign = await _api.PresignUploadAsync(new("avatar", contentType, null));
            var uploaded = await _api.UploadImageAsync(presign.Data.UploadUrl, imageStream, contentType);
            if (!uploaded) return null;

            await _api.ConfirmAvatarUploadAsync(presign.Data.Key);
            return presign.Data.PublicUrl;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UploadAvatar failed: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> UploadEventBannerAsync(string eventId, Stream imageStream, string contentType)
    {
        try
        {
            var presign = await _api.PresignUploadAsync(new("event_banner", contentType, eventId));
            var uploaded = await _api.UploadImageAsync(presign.Data.UploadUrl, imageStream, contentType);
            if (!uploaded) return null;

            await _api.ConfirmEventBannerAsync(eventId, presign.Data.Key);
            return presign.Data.PublicUrl;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UploadEventBanner failed: {ex.Message}");
            return null;
        }
    }
}
