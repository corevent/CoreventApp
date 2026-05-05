using System;
using System.Text.Json;
using System.Threading.Tasks;
using CoreventApp.Models;
using Microsoft.Maui.Storage;

namespace CoreventApp.Services;

public class MockAuthService : IAuthService
{
  private const string AuthStorageKey = "logged_user_data";

  public async Task<User?> LoginAsync(string email, string password)
  {
    // Simulação de delay de rede
    await Task.Delay(1000);

    if (email == "teste@email.com" && password == "123456")
    {
      var user = new User
      {
        Id = Guid.NewGuid().ToString(),
        Name = "Teste da Silva",
        Email = email,
        AvatarUrl = "profile_default_icon.png", // Imagem padrão
        CreatedAt = DateTime.UtcNow
      };

      var userJson = JsonSerializer.Serialize(user);
      await SecureStorage.Default.SetAsync(AuthStorageKey, userJson);

      return user;
    }

    return null;
  }

  public async Task<User?> GetCurrentUserAsync()
  {
    var userJson = await SecureStorage.Default.GetAsync(AuthStorageKey);

    if (!string.IsNullOrEmpty(userJson))
    {
      try
      {
        return JsonSerializer.Deserialize<User>(userJson);
      }
      catch
      {
        return null;
      }
    }

    return null;
  }

  public void Logout()
  {
    SecureStorage.Default.Remove(AuthStorageKey);
  }

  public Task LogoutAsync()
  {
    Logout();
    return Task.CompletedTask;
  }
}