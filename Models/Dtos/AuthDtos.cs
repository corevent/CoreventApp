namespace CoreventApp.Models.Dtos;

public record LoginDto(string Email, string Password);
public record AuthTokensDto(string AccessToken, string RefreshToken);
public record RefreshTokenDto(string RefreshToken);
public record EmailDto(string Email);
public record ResetPasswordDto(string Email, string Code, string NewPassword);
public record MessageDto(string Message);
public record RegisterDto(
    string Name,
    string PhoneNumber,
    string AvatarUrl,
    string Email,
    string Password,
    string BirthDate,
    string DocumentType,
    string Document,
    string VerifyEmailCode);
