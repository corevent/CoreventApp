using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class PaymentInfoService
{
    private readonly PaymentInfoApiClient _api;
    private readonly IAuthService _authService;

    public PaymentInfoService(PaymentInfoApiClient api, IAuthService authService)
    {
        _api = api;
        _authService = authService;
    }

    private string GetUserId()
    {
        return _authService.CurrentCachedUser?.Id
            ?? throw new InvalidOperationException("User not authenticated");
    }

    public async Task<OrganizerPaymentInfoDataDto?> CreateAsync(CreateOrganizerPaymentInfoDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _api.CreateAsync(userId, dto);
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Create payment info failed: {ex.Message}");
            return null;
        }
    }

    public async Task<List<OrganizerPaymentInfoDataDto>> GetAllAsync()
    {
        try
        {
            var userId = GetUserId();
            var page = await _api.GetAllAsync(userId, 1, 50);
            if (page.Data.Count == 0)
                return new List<OrganizerPaymentInfoDataDto>();

            var ids = page.Data.Select(x => x.Id).ToList();
            var results = new List<OrganizerPaymentInfoDataDto>();
            foreach (var id in ids)
            {
                var item = await _api.GetByIdAsync(id);
                results.Add(item.Data);
            }
            return results;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get all payment info failed: {ex.Message}");
            return new List<OrganizerPaymentInfoDataDto>();
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
            Debug.WriteLine($"Delete payment info failed: {ex.Message}");
            return false;
        }
    }
}
