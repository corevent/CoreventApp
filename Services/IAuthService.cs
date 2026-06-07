using CoreventApp.Models;

namespace CoreventApp.Services;

public interface IAuthService
{
    User? CurrentCachedUser { get; }

    Task<User?> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<User?> GetCurrentUserAsync(bool forceRefresh = false);

    Task<bool> UpdatePasswordAsync(string currentPassword, string newPassword);
    Task<bool> UpdateProfileAsync(string name, string? phoneNumber, string? avatarUrl);

    Task SendVerificationEmailAsync(string email);
    Task<User?> CreateUserAsync(string name, string email, string password,
        string cpf, string birthDate, string code);

    Task SendResetCodeAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string code, string newPassword);
}
