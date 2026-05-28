namespace CoreventApp.Models.Dtos;

public record LoginDto(string Email, string Password);
public record AuthTokensDto(string AccessToken, string RefreshToken);
public record RefreshTokenDto(string RefreshToken);
public record EmailDto(string Email);
public record ResetPasswordDto(string Email, string Code, string NewPassword);
public record MessageDto(string Message);
