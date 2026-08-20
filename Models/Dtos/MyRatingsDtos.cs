namespace CoreventApp.Models.Dtos;

public record MyRatingItemDto(
    string EventId,
    string EventTitle,
    string BannerUrl,
    double AverageRating,
    int UserRating);

public record MyRatingsListPageDto(
    List<MyRatingItemDto> Data,
    PaginationMetaDto Meta);
