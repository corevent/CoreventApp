namespace CoreventApp.Models.Dtos;

public record PresignUploadDto(string Purpose, string ContentType, string? EventId);

public record PresignUploadDataDto(string UploadUrl, string Key, string PublicUrl, int ExpiresIn);

public record PresignUploadResponseDto(PresignUploadDataDto Data);

public record ConfirmImageUploadDto(string Key);
