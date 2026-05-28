using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class EventsService
{
    private readonly EventsApiClient _api;

    public EventsService(EventsApiClient api)
    {
        _api = api;
    }

    public async Task<EventDetailDto?> CreateAsync(CreateEventDto dto)
    {
        try
        {
            var result = await _api.CreateAsync(dto);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Create event failed: {ex.Message}");
            return null;
        }
    }

    public async Task<EventListPageDto> GetAllAsync(
        int page = 1, int limit = 10,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        string? status = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        try
        {
            return await _api.GetAllAsync(page, limit, search, category, startDate, status, isAdultOnly, stateId, cityId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get all events failed: {ex.Message}");
            return new EventListPageDto(new List<EventListItemDto>(),
                new PaginationMetaDto(0, 0, page, limit));
        }
    }

    public async Task<EventDetailDto?> GetByIdAsync(string id)
    {
        try
        {
            var result = await _api.GetByIdAsync(id);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get event by id failed: {ex.Message}");
            return null;
        }
    }

    public async Task<EventDetailDto?> UpdateAsync(string id, UpdateEventDto dto)
    {
        try
        {
            var result = await _api.UpdateAsync(id, dto);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Update event failed: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            await _api.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Delete event failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> CancelAsync(string id)
    {
        try
        {
            await _api.CancelAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Cancel event failed: {ex.Message}");
            return false;
        }
    }
}
