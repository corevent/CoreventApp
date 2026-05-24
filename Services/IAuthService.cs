using CoreventApp.Models;

namespace CoreventApp.Services;

public interface IAuthService
{
    Task<User?> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<User?> GetCurrentUserAsync();
    Task<bool> UpdateEmailAsync(string newEmail, string currentPassword);
    Task<bool> UpdatePasswordAsync(string currentPassword, string newPassword);
    Task<bool> UpdateProfileAsync(string name, string cpf, string birthDate);

    Task SendVerificationEmailAsync(string email);
    Task<bool> VerifyCodeAsync(string email, string code);
    Task<bool> RegisterUserAsync(string name, string email, string password, string cpf, string birthDate);

    Task SendResetCodeAsync(string email);
    Task<bool> VerifyResetCodeAsync(string email, string code);
    Task<bool> ResetPasswordAsync(string email, string newPassword);
}
