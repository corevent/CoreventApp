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
}
