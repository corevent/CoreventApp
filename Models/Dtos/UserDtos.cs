namespace CoreventApp.Models.Dtos;

public record CreateUserDto(
    string Name,
    string PhoneNumber,
    string AvatarUrl,
    string Email,
    string Password,
    string BirthDate,
    string Cpf,
    string VerifyEmailCode);

public record UpdateUserDto(string? Name, string? PhoneNumber, string? AvatarUrl);

public record UpdatePassDto(string CurrentPassword, string NewPassword);

public record UserDataDto(
    string Id,
    string Name,
    string Email,
    string Cpf,
    string BirthDate,
    string PhoneNumber,
    string AvatarUrl,
    DateTime CreatedAt);

public record UserResponseDto(UserDataDto Data);
