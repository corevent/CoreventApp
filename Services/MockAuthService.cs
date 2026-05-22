using System;
using System.Text.Json;
using System.Threading.Tasks;
using CoreventApp.Models;
using Microsoft.Maui.Storage;

namespace CoreventApp.Services;

public class MockAuthService : IAuthService
{
    private const string AuthStorageKey = "logged_user_data";
    private string _mockPassword = "123456";

    public async Task<User?> LoginAsync(string email, string password)
    {
        if (email == "teste@email.com" && password == _mockPassword)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Teste da Silva",
                Email = email,
                CPF = "123.456.789-00",
                BirthDate = "01/01/1990",
                Cellphone = "(11) 91234-5678",
                AvatarUrl = "profile_default_icon.png",
                CreatedAt = DateTime.UtcNow
            };

            await SaveUserAsync(user);
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

    public async Task LogoutAsync()
    {
        SecureStorage.Default.Remove(AuthStorageKey);
        await Task.CompletedTask;
    }

    public async Task<bool> UpdateEmailAsync(string newEmail, string currentPassword)
    {
        if (currentPassword != _mockPassword) return false;

        var user = await GetCurrentUserAsync();
        if (user == null) return false;

        user.Email = newEmail;
        await SaveUserAsync(user);
        return true;
    }

    public async Task<bool> UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        if (currentPassword != _mockPassword) return false;

        _mockPassword = newPassword;
        return true;
    }

    public async Task<bool> UpdateProfileAsync(string name, string cpf, string birthDate)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return false;

        user.Name = name;
        user.CPF = cpf;
        user.BirthDate = birthDate;
        await SaveUserAsync(user);
        return true;
    }

    private async Task SaveUserAsync(User user)
    {
        var userJson = JsonSerializer.Serialize(user);
        await SecureStorage.Default.SetAsync(AuthStorageKey, userJson);
    }
}
