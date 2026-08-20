namespace CoreventApp.Models.Dtos;

public record FavoriteDataDto(string Id, string UserId, string EventId);
public record FavoriteResponseDto(FavoriteDataDto Data);
