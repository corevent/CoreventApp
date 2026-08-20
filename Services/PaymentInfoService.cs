using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class PaymentInfoService
{
    private readonly PaymentInfoApiClient _api;

    public PaymentInfoService(PaymentInfoApiClient api)
    {
        _api = api;
    }

    public async Task<OrganizerPaymentInfoDataDto?> CreateAsync(CreateOrganizerPaymentInfoDto dto)
    {
        try
        {
            var result = await _api.CreateAsync(dto);
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
            var page = await _api.GetAllAsync(1, 50);
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
