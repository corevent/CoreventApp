using CoreventApp.Models;

namespace CoreventApp.Services;

public interface IAuthService
{
  Task<User?> LoginAsync(string email, string password);
  Task LogoutAsync();
  Task<User?> GetCurrentUserAsync();
}