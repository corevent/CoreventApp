namespace CoreventApp.Models.Dtos;

// Ticket Types
public record CreateTicketTypeDto(
    string Name,
    decimal Price,
    int TotalQuantity,
    DateTime StartDate,
    DateTime EndDate);

public record TicketTypeDataDto(
    string Id,
    string EventId,
    string Name,
    decimal Price,
    int TotalQuantity,
    int AvailableQuantity,
    DateTime StartDate,
    DateTime EndDate);

public record TicketTypeListMeta(int TotalItems, int TotalPages, int CurrentPage, int ItemsPerPage);

public record TicketTypeListPageDto(List<TicketTypeDataDto> Data, TicketTypeListMeta Meta);

public record TicketTypeResponseDto(TicketTypeDataDto Data);

public record UpdateTicketTypeDto(
    string? Name = null,
    decimal? Price = null,
    int? TotalQuantity = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null);

// Orders
public record ItemsDto(string TicketTypeId, int Quantity);

public record CreateOrderDto(List<ItemsDto> Items);

public record CheckoutDataDto(string Rel, string Href, string Method);

public record OrderDataDto(
    string OrderId,
    List<CheckoutDataDto> CheckoutLinks,
    List<string> QrCodes,
    List<string> TicketIds);

public record OrderResponseDto(OrderDataDto Data);
