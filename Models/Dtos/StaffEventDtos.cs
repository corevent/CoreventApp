namespace CoreventApp.Models.Dtos;

public record StaffEventListItemDto(
    string Id,
    string Title,
    int MaxParticipants,
    string? CityName,
    string? StateAcronym,
    string LocationName,
    DateTime StartDate,
    DateTime EndDate,
    string Category,
    bool IsAdultOnly,
    string Status,
    OrganizerInfoDto Organizer,
    string AccessLevel);

public record StaffEventListPageDto(List<StaffEventListItemDto> Data, PaginationMetaDto Meta);
