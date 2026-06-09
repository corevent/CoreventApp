namespace CoreventApp.Models.Dtos;

// Check-in
public record CheckinDto(string QrToken);

public record CheckinTicketTypeDto(string Id, string Name, decimal Price);

public record CheckinEventDto(string Id, string Title);

public record CheckinOrderDto(string Id, string Status, string GatewayTransactionId);

public record CheckinUserDto(string Id, string Name, string Email);

public record CheckinStaffUserDto(string Id, string Name);

public record CheckinDataDto(
    string TicketId,
    string TicketTypeId,
    string Status,
    DateTime CheckinAt,
    CheckinTicketTypeDto TicketType,
    CheckinEventDto Event,
    CheckinOrderDto Order,
    CheckinUserDto User,
    CheckinStaffUserDto CheckedInBy);

public record CheckinResponseDto(CheckinDataDto Data);

// User tickets
public record UserTicketTypeDto(string Id, string Name, decimal Price);

public record UserTicketEventDto(string Id, string Title);

public record UserTicketOrderDto(string Id, string Status);

public record UserTicketDataDto(
    string Id,
    string EventId,
    string TicketTypeId,
    string Status,
    DateTime? CheckinAt,
    string QrToken,
    UserTicketTypeDto TicketType,
    UserTicketEventDto Event,
    UserTicketOrderDto Order);

public record MyTicketsResponseDto(List<UserTicketDataDto> Data);

public record PaginateMyTicketsDto(
    List<UserTicketDataDto> Data,
    PaginationMetaDto Meta);

public record QueryMyTicketsDto(int Page = 1, int Limit = 100, string? EventId = null);
