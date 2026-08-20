using System.Text;
using System.Text.Json;
using CoreventApp.Models.Dtos;

namespace CoreventApp.Services.Api;

public class OrdersApiClient
{
    private readonly HttpClient _http;

    public OrdersApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<OrderResponseDto> CreateAsync(string eventId, CreateOrderDto dto)
    {
        var json = JsonSerializer.Serialize(dto, JsonConfig.Options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"/api/events/{eventId}/orders", content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderResponseDto>(body, JsonConfig.Options)!;
    }

    public async Task<PaginateMyOrdersDto> GetMyOrdersAsync(int page = 1, int limit = 20)
    {
        var response = await _http.GetAsync($"/api/events/my/orders?page={page}&limit={limit}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PaginateMyOrdersDto>(body, JsonConfig.Options)!;
    }

    public async Task<OrderDetailsResponseDto> GetByIdAsync(string orderId)
    {
        var response = await _http.GetAsync($"/api/events/orders/{orderId}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderDetailsResponseDto>(body, JsonConfig.Options)!;
    }
}
