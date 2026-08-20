using System.Collections.Concurrent;
using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class FavoritesService
{
    private readonly FavoritesApiClient _api;
    private readonly ConcurrentDictionary<string, string> _favoriteIdByEventId = new();

    public FavoritesService(FavoritesApiClient api)
    {
        _api = api;
    }

    public bool IsFavorite(string eventId)
    {
        return _favoriteIdByEventId.ContainsKey(eventId);
    }

    public string? GetFavoriteId(string eventId)
    {
        return _favoriteIdByEventId.TryGetValue(eventId, out var favoriteId) ? favoriteId : null;
    }

    public void SetFavorites(IEnumerable<EventListItemDto> events)
    {
        _favoriteIdByEventId.Clear();
        foreach (var ev in events)
        {
            _favoriteIdByEventId.TryAdd(ev.Id, ev.FavoriteId ?? string.Empty);
        }
    }

    public void SetFavoriteIdByEventId(string eventId, string favoriteId)
    {
        _favoriteIdByEventId[eventId] = favoriteId;
    }

    public void RemoveFromCache(string eventId)
    {
        _favoriteIdByEventId.TryRemove(eventId, out _);
    }

    public void ClearCache()
    {
        _favoriteIdByEventId.Clear();
    }

    public async Task<FavoriteDataDto?> AddFavoriteAsync(string eventId)
    {
        try
        {
            var result = await _api.CreateAsync(eventId);
            _favoriteIdByEventId[eventId] = result.Data.Id;
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Add favorite failed: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> RemoveFavoriteAsync(string eventId)
    {
        try
        {
            var favoriteId = GetFavoriteId(eventId);
            if (favoriteId is null) return false;

            await _api.DeleteAsync(favoriteId);
            _favoriteIdByEventId.TryRemove(eventId, out _);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Remove favorite failed: {ex.Message}");
            return false;
        }
    }
}
