using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class TicketsService
{
    private readonly TicketsApiClient _api;

    public TicketsService(TicketsApiClient api)
    {
        _api = api;
    }

    public async Task<PaginateMyTicketsDto> GetMyTicketsAsync(int page = 1, int limit = 100, string? eventId = null)
    {
        try
        {
            return await _api.GetMyTicketsAsync(page, limit, eventId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my tickets failed: {ex.Message}");
            return new PaginateMyTicketsDto(new List<UserTicketDataDto>(),
                new PaginationMetaDto(0, 0, page, limit));
        }
    }

    public async Task<MyTicketsResponseDto> GetMyTicketsByEventAsync(string eventId)
    {
        try
        {
            return await _api.GetMyTicketsByEventAsync(eventId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my tickets by event failed: {ex.Message}");
            return new MyTicketsResponseDto(new List<UserTicketDataDto>());
        }
    }
}
