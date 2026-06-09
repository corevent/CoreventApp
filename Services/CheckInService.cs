using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class CheckInService
{
    private readonly CheckInApiClient _api;

    public CheckInService(CheckInApiClient api)
    {
        _api = api;
    }

    public async Task<CheckinDataDto?> CheckinAsync(string eventId, string qrToken)
    {
        try
        {
            var result = await _api.CheckinAsync(eventId, new CheckinDto(qrToken));
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CheckIn failed: {ex.Message}");
            return null;
        }
    }
}
