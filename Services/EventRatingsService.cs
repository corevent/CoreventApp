using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class EventRatingsService
{
    private readonly EventRatingsApiClient _api;

    public EventRatingsService(EventRatingsApiClient api)
    {
        _api = api;
    }

    public async Task<MyRatingsListPageDto> GetMyRatingsAsync(int page = 1, int limit = 50)
    {
        try
        {
            return await _api.GetMyRatingsAsync(page, limit);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my ratings failed: {ex.Message}");
            return new MyRatingsListPageDto(new List<MyRatingItemDto>(),
                new PaginationMetaDto(0, 0, page, limit));
        }
    }
}
