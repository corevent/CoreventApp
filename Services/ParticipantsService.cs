using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class ParticipantsService
{
    private readonly ParticipantsApiClient _api;

    public ParticipantsService(ParticipantsApiClient api)
    {
        _api = api;
    }

    public async Task<ParticipantListPageDto> GetAllAsync(string eventId, int page = 1, int limit = 100)
    {
        try
        {
            return await _api.GetAllAsync(eventId, page, limit);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ParticipantsService.GetAllAsync failed: {ex.Message}");
            return new ParticipantListPageDto(
                new List<ParticipantDataDto>(),
                new ParticipantPaginationMetaDto(0, 0, page, limit));
        }
    }
}
