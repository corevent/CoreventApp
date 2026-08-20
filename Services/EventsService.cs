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

    public async Task<EventListPageDto> GetMyOrganizerEventsAsync(
        int page, int limit,
        string status,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        try
        {
            return await _api.GetMyOrganizerEventsAsync(page, limit, status, search, category, startDate, isAdultOnly, stateId, cityId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my organizer events failed: {ex.Message}");
            return new EventListPageDto(new List<EventListItemDto>(),
                new PaginationMetaDto(0, 0, page, limit));
        }
    }

    public async Task<EventListPageDto> GetMyOrganizerEventsAllAsync(
        int page = 1, int limit = 100,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        try
        {
            var statuses = new[] { "draft", "opened", "going", "canceled", "finished" };
            var tasks = statuses.Select(s =>
                _api.GetMyOrganizerEventsAsync(page, limit, s, search, category, startDate, isAdultOnly, stateId, cityId));
            var results = await Task.WhenAll(tasks);
            var combined = results.SelectMany(r => r.Data).ToList();
            return new EventListPageDto(combined,
                new PaginationMetaDto(combined.Count, 1, page, combined.Count));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my organizer events all failed: {ex.Message}");
            return new EventListPageDto(new List<EventListItemDto>(),
                new PaginationMetaDto(0, 0, page, limit));
        }
    }

    public async Task<StaffEventListPageDto> GetMyStaffEventsAsync(
        int page, int limit,
        string status,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        try
        {
            return await _api.GetMyStaffEventsAsync(page, limit, status, search, category, startDate, isAdultOnly, stateId, cityId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my staff events failed: {ex.Message}");
            return new StaffEventListPageDto(new List<StaffEventListItemDto>(),
                new PaginationMetaDto(0, 0, page, limit));
        }
    }

    public async Task<StaffEventListPageDto> GetMyStaffEventsAllAsync(
        int page = 1, int limit = 100,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        try
        {
            var statuses = new[] { "opened", "going", "finished" };
            var tasks = statuses.Select(s =>
                _api.GetMyStaffEventsAsync(page, limit, s, search, category, startDate, isAdultOnly, stateId, cityId));
            var results = await Task.WhenAll(tasks);
            var combined = results.SelectMany(r => r.Data).ToList();
            return new StaffEventListPageDto(combined,
                new PaginationMetaDto(combined.Count, 1, page, combined.Count));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my staff events all failed: {ex.Message}");
            return new StaffEventListPageDto(new List<StaffEventListItemDto>(),
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

    public async Task<EventDetailDto?> UpdatePartialAsync(string id, Dictionary<string, object?> payload)
    {
        try
        {
            var result = await _api.UpdatePartialAsync(id, payload);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Update event partial failed: {ex.Message}");
            return null;
        }
    }

    public async Task<EventDetailDto?> UpdateStatusAsync(string id, string status)
    {
        try
        {
            var result = await _api.UpdateStatusAsync(id, status);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Update event status failed: {ex.Message}");
            return null;
        }
    }

    public async Task<EventListPageDto> GetMyFavoriteEventsAsync(
        int page = 1, int limit = 100,
        string? search = null,
        string? category = null,
        DateTime? startDate = null,
        bool? isAdultOnly = null,
        int? stateId = null,
        int? cityId = null)
    {
        try
        {
            var statuses = new[] { "opened", "going", "finished" };
            var tasks = statuses.Select(s =>
                _api.GetMyFavoriteEventsAsync(page, limit, s, search, category, startDate, isAdultOnly, stateId, cityId));
            var results = await Task.WhenAll(tasks);
            var combined = results.SelectMany(r => r.Data).ToList();
            return new EventListPageDto(combined,
                new PaginationMetaDto(combined.Count, 1, page, combined.Count));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my favorite events failed: {ex.Message}");
            return new EventListPageDto(new List<EventListItemDto>(),
                new PaginationMetaDto(0, 0, page, limit));
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
