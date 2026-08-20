using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class AgePolicyService
{
    private readonly AgePoliciesApiClient _api;

    public AgePolicyService(AgePoliciesApiClient api)
    {
        _api = api;
    }

    public async Task<AgePolicyDataDto?> GetActivePolicyAsync()
    {
        try
        {
            var result = await _api.GetActivePolicyAsync();
            return result.Data;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GetActivePolicy failed: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CheckIfUserHasAcceptedAsync()
    {
        try
        {
            var result = await _api.CheckAcceptanceAsync();
            return result.Data.UserHasAccepted;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CheckAcceptance failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AcceptAgePolicyAsync()
    {
        try
        {
            await _api.AcceptPolicyAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"AcceptAgePolicy failed: {ex.Message}");
            return false;
        }
    }
}
