namespace CoreventApp.Models.Dtos;

public record UserInfoDto(string Id, string Name, string Email, string AvatarUrl);

// Event Staff
public record ListEventStaffDto(
    string Id,
    string AccessLevel,
    string InvitationStatus,
    string StaffInvitationId,
    DateTime CreatedAt,
    UserInfoDto User);

public record PaginateEventStaffDto(List<ListEventStaffDto> Data, PaginationMetaDto Meta);

public record EventStaffDataDto(
    string Id,
    string UserId,
    string AccessLevel,
    string InvitationStatus,
    string StaffInvitationId,
    DateTime CreatedAt);

public record EventStaffResponseDto(EventStaffDataDto Data);

// Invitations
public record CreateEventStaffInvitationDto(string Email, string OriginalAccessLevel);

public record EventStaffInvitationDataDto(
    string Email,
    string OriginalAccessLevel,
    string Id,
    string UserId,
    string EventId,
    string InvitationStatus,
    DateTime CreatedAt);

public record EventStaffInvitationResponseDto(EventStaffInvitationDataDto Data);

public record ListEventStaffInvitationDto(
    string Id,
    string UserId,
    string EventId,
    string OriginalAccessLevel,
    string InvitationStatus,
    UserInfoDto User);

public record PaginateEventStaffInvitationsDto(
    List<ListEventStaffInvitationDto> Data,
    PaginationMetaDto Meta);


