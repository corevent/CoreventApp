namespace CoreventApp.Models.Dtos;

public record CreateOrganizerPaymentInfoDto(
    string Description,
    string? BranchNumber,
    string? BranchDigit,
    string? AccountNumber,
    string? AccountDigit,
    string? PixKey,
    string? PixType,
    string? BankCode);

public record UpdateOrganizerPaymentInfoDto(
    string? Description,
    string? BranchNumber,
    string? BranchDigit,
    string? AccountNumber,
    string? AccountDigit,
    string? PixKey,
    string? PixType,
    string? BankCode);

public record OrganizerPaymentInfoDataDto(
    string Id,
    string UserId,
    string Description,
    string? BranchNumber,
    string? BranchDigit,
    string? AccountNumber,
    string? AccountDigit,
    string? PixKey,
    string? PixType,
    string? BankCode);

public record OrganizerPaymentInfoResDto(OrganizerPaymentInfoDataDto Data);

public record ListOrganizerPaymentInfoDto(string Id, string Description);

public record OrganizerPaymentInfoPageDto(
    List<ListOrganizerPaymentInfoDto> Data,
    PaginationMetaDto Meta);
