namespace CoreventApp.Models.Dtos;

public record CreateEventRatingDto(int Rating);
public record EventRatingDataDto(string Id, string EventId, string UserId, int Rating, DateTime CreatedAt);
public record EventRatingResponseDto(EventRatingDataDto Data);
