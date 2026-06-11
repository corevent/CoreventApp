namespace CoreventApp.Models.Dtos;

public record ParticipantDataDto(
    string Id,
    string Name,
    string Email,
    int TicketsCount);

public record ParticipantPaginationMetaDto(
    int TotalItems,
    int TotalPages,
    int CurrentPage,
    int ItemsPerPage);

public record ParticipantListPageDto(
    List<ParticipantDataDto> Data,
    ParticipantPaginationMetaDto Meta);
