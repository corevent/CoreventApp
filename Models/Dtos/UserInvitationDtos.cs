namespace CoreventApp.Models.Dtos;

public record EventRefDto(string Id, string Title, UserInfoDto Organizer);

public record UserInvitationDto(
    string Id,
    string UserId,
    EventRefDto Event,
    string OriginalAccessLevel,
    string InvitationStatus,
    DateTime CreatedAt);

public record UserInvitationPageDto(
    List<UserInvitationDto> Data,
    PaginationMetaDto Meta);
