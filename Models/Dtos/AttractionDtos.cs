namespace CoreventApp.Models.Dtos;

public record CreateAttractionDto(
    string Title,
    string Guest,
    DateTime StartDate,
    DateTime EndDate);

public record UpdateAttractionDto(
    string? Title,
    string? Guest,
    DateTime? StartDate,
    DateTime? EndDate);

public record AttractionDto(
    string Id,
    string Title,
    string Guest,
    DateTime StartDate,
    DateTime EndDate,
    DateTime? CreatedAt);

public record AttractionResponseDto(AttractionDto Data);

public record AttractionListPageDto(List<AttractionDto> Data, PaginationMetaDto Meta);
