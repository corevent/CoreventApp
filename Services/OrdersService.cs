using System.Diagnostics;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.Services;

public class OrdersService
{
    private readonly OrdersApiClient _api;

    public OrdersService(OrdersApiClient api)
    {
        _api = api;
    }

    public async Task<PaginateMyOrdersDto> GetMyOrdersAsync(int page = 1, int limit = 20)
    {
        try
        {
            return await _api.GetMyOrdersAsync(page, limit);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get my orders failed: {ex.Message}");
            return new PaginateMyOrdersDto(new List<MyOrdersDataDto>(),
                new PaginationMetaDto(0, 0, page, limit));
        }
    }

    public async Task<OrderDetailsResponseDto?> GetByIdAsync(string orderId)
    {
        try
        {
            return await _api.GetByIdAsync(orderId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Get order by id failed: {ex.Message}");
            return null;
        }
    }
}
