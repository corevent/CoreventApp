namespace CoreventApp.Models.Dtos;

public record AgePolicyDataDto(string Id, string Description, double Version, bool IsActive);

public record AgePolicyResponseDto(AgePolicyDataDto Data);

public record CheckAcceptanceDataDto(bool UserHasAccepted);

public record CheckAcceptanceResponseDto(CheckAcceptanceDataDto Data);

public record AgePolicyAcceptanceDataDto(string Id, string UserId, string AgePolicyId, DateTime CreatedAt);

public record AgePolicyAcceptanceResponseDto(AgePolicyAcceptanceDataDto Data);
