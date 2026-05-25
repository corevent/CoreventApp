using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using CoreventApp.Models;
using Microsoft.Maui.Storage;

namespace CoreventApp.Services;

public class MockAuthService : IAuthService
{
    private const string AuthStorageKey = "logged_user_data";
    private const string RegisteredUsersKey = "registered_users";
    private string _mockPassword = "123456";

    // Pending verification state
    private static string? _pendingEmail;
    private static string? _pendingCode;
    private static string? _pendingPassword;
    private static string? _pendingName;
    private static string? _pendingCpf;
    private static string? _pendingBirthDate;
    private static DateTime _pendingCreatedAt;

    public async Task<User?> LoginAsync(string email, string password)
    {
        // Check mock default user
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

        // Check registered users (after email verification)
        var registered = await GetRegisteredUsersAsync();
        if (registered.TryGetValue(email, out var savedPassword) && savedPassword == password)
        {
            var user = await GetCurrentUserAsync();
            if (user != null && user.Email == email)
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

    public Task SendVerificationEmailAsync(string email)
    {
        _pendingEmail = email;
        _pendingCode = Random.Shared.Next(100000, 999999).ToString();

        Debug.WriteLine($"[MockAuthService] Verification code for {email}: {_pendingCode}");

        return Task.CompletedTask;
    }

    public async Task<bool> VerifyCodeAsync(string email, string code)
    {
        if (_pendingEmail != email || _pendingCode == null)
            return false;

        if (code != _pendingCode)
            return false;

        // Code is valid — create the user and log them in
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = _pendingName ?? "Usuário",
            Email = email,
            CPF = _pendingCpf ?? string.Empty,
            BirthDate = _pendingBirthDate ?? string.Empty,
            AvatarUrl = "profile_default_icon.png",
            CreatedAt = _pendingCreatedAt == default ? DateTime.UtcNow : _pendingCreatedAt
        };

        await SaveUserAsync(user);

        // Store in registered users for login
        if (_pendingPassword != null)
        {
            await AddRegisteredUserAsync(email, _pendingPassword);
        }

        ClearPending();
        return true;
    }

    public async Task<bool> RegisterUserAsync(string name, string email, string password, string cpf, string birthDate)
    {
        _pendingName = name;
        _pendingEmail = email;
        _pendingPassword = password;
        _pendingCpf = cpf;
        _pendingBirthDate = birthDate;
        _pendingCreatedAt = DateTime.UtcNow;

        await SendVerificationEmailAsync(email);
        return true;
    }

    public async Task SendResetCodeAsync(string email)
    {
        var registered = await GetRegisteredUsersAsync();
        if (registered.ContainsKey(email))
        {
            _pendingEmail = email;
            _pendingCode = Random.Shared.Next(100000, 999999).ToString();
            System.Diagnostics.Debug.WriteLine($"[MockAuthService] Reset code for {email}: {_pendingCode}");
        }
    }

    public Task<bool> VerifyResetCodeAsync(string email, string code)
    {
        if (_pendingEmail != email || _pendingCode == null)
            return Task.FromResult(false);

        if (code != _pendingCode)
            return Task.FromResult(false);

        _pendingEmail = null;
        _pendingCode = null;
        return Task.FromResult(true);
    }

    public async Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        var registered = await GetRegisteredUsersAsync();

        if (!registered.ContainsKey(email))
            return false;

        registered[email] = newPassword;
        var json = JsonSerializer.Serialize(registered);
        await SecureStorage.Default.SetAsync(RegisteredUsersKey, json);

        return true;
    }

    private async Task SaveUserAsync(User user)
    {
        var userJson = JsonSerializer.Serialize(user);
        await SecureStorage.Default.SetAsync(AuthStorageKey, userJson);
    }

    private async Task<Dictionary<string, string>> GetRegisteredUsersAsync()
    {
        var json = await SecureStorage.Default.GetAsync(RegisteredUsersKey);
        if (string.IsNullOrEmpty(json))
            return new Dictionary<string, string>();

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    private async Task AddRegisteredUserAsync(string email, string password)
    {
        var users = await GetRegisteredUsersAsync();
        users[email] = password;
        var json = JsonSerializer.Serialize(users);
        await SecureStorage.Default.SetAsync(RegisteredUsersKey, json);
    }

    private static void ClearPending()
    {
        _pendingEmail = null;
        _pendingCode = null;
        _pendingPassword = null;
        _pendingName = null;
        _pendingCpf = null;
        _pendingBirthDate = null;
        _pendingCreatedAt = default;
    }
}
