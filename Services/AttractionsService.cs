using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class AttractionsService
{
    private readonly AttractionsApiClient _api;

    public AttractionsService(AttractionsApiClient api)
    {
        _api = api;
    }

    public async Task<AttractionDto?> CreateAsync(string eventId, CreateAttractionDto dto)
    {
        try
        {
            var result = await _api.CreateAsync(eventId, dto);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Create attraction failed: {ex.Message}");
            return null;
        }
    }

    public async Task<AttractionListPageDto> GetAllAsync(string eventId, int page = 1, int limit = 10)
    {
        try
        {
            return await _api.GetAllAsync(eventId, page, limit);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get attractions failed: {ex.Message}");
            return new AttractionListPageDto(new List<AttractionDto>(),
                new PaginationMetaDto(0, 0, page, limit));
        }
    }

    public async Task<AttractionDto?> GetByIdAsync(string attractionId)
    {
        try
        {
            var result = await _api.GetByIdAsync(attractionId);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get attraction by id failed: {ex.Message}");
            return null;
        }
    }

    public async Task<AttractionDto?> UpdateAsync(string attractionId, UpdateAttractionDto dto)
    {
        try
        {
            var result = await _api.UpdateAsync(attractionId, dto);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Update attraction failed: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string attractionId)
    {
        try
        {
            await _api.DeleteAsync(attractionId);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Delete attraction failed: {ex.Message}");
            return false;
        }
    }
}
