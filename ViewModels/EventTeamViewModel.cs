using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Helpers;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventName), "EventName")]
public partial class EventTeamViewModel : ObservableObject
{
    private readonly EventStaffApiClient _staffApi;
    private readonly StaffInvitesApiClient _invitesApi;
    private string? _eventId;

    public string? EventId
    {
        get => _eventId;
        set
        {
            _eventId = value;
            if (value is not null) _ = LoadDataAsync(value);
        }
    }

    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string InviteEmail { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string SelectedRole { get; set; } = "Credenciamento";

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public ObservableCollection<TeamMember> PendingInvites { get; } = new();
    public ObservableCollection<TeamMember> ActiveTeam { get; } = new();

    public int PendingCount => PendingInvites.Count;
    public int ActiveCount => ActiveTeam.Count;

    public EventTeamViewModel(EventStaffApiClient staffApi, StaffInvitesApiClient invitesApi)
    {
        _staffApi = staffApi;
        _invitesApi = invitesApi;
    }

    private async Task LoadDataAsync(string eventId)
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var staffTask = _staffApi.GetAllAsync(eventId, page: 1, limit: 100);
            var invitesTask = _invitesApi.GetAllAsync(eventId, page: 1, limit: 100);

            await Task.WhenAll(staffTask, invitesTask);

            var staffResult = staffTask.Result;
            var invitesResult = invitesTask.Result;

            PendingInvites.Clear();
            foreach (var invite in invitesResult.Data)
            {
                PendingInvites.Add(new TeamMember
                {
                    Email = invite.Email,
                    Role = invite.OriginalAccessLevel == "checkin" ? "Credenciamento" : "Organização",
                    IsPending = true,
                    InvitationId = invite.Id
                });
            }

            ActiveTeam.Clear();
            foreach (var staff in staffResult.Data)
            {
                ActiveTeam.Add(new TeamMember
                {
                    Name = staff.User.Name ?? staff.User.Email,
                    Email = staff.User.Email,
                    Role = staff.AccessLevel == "checkin" ? "Credenciamento" : "Organização",
                    IsPending = false,
                    StaffId = staff.Id
                });
            }

            OnPropertyChanged(nameof(PendingCount));
            OnPropertyChanged(nameof(ActiveCount));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"EventTeam LoadDataAsync failed: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void ToggleRole(string role)
    {
        SelectedRole = role;
    }

    [RelayCommand]
    private async Task InviteAsync()
    {
        if (string.IsNullOrWhiteSpace(InviteEmail) || _eventId is null) return;

        if (!ValidationHelper.IsValidEmail(InviteEmail))
        {
            await Shell.Current.DisplayAlertAsync("Erro", "Informe um e-mail válido.", "OK");
            return;
        }

        var accessLevel = SelectedRole == "Credenciamento" ? "checkin" : "readonly";
        var dto = new CreateEventStaffInvitationDto(InviteEmail.Trim(), accessLevel);

        try
        {
            await _invitesApi.CreateAsync(_eventId, dto);

            PendingInvites.Add(new TeamMember
            {
                Email = InviteEmail.Trim(),
                Role = SelectedRole,
                IsPending = true
            });

            InviteEmail = string.Empty;
            OnPropertyChanged(nameof(PendingCount));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"EventTeam InviteAsync failed: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task RemoveMember(TeamMember member)
    {
        try
        {
            if (member.IsPending && !string.IsNullOrEmpty(member.InvitationId))
            {
                await _invitesApi.CancelAsync(member.InvitationId);
                PendingInvites.Remove(member);
                OnPropertyChanged(nameof(PendingCount));
            }
            else if (!string.IsNullOrEmpty(member.StaffId))
            {
                await _staffApi.DeleteAsync(member.StaffId);
                ActiveTeam.Remove(member);
                OnPropertyChanged(nameof(ActiveCount));
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"EventTeam RemoveMember failed: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}

public partial class TeamMember : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Role { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsPending { get; set; }

    public string? StaffId { get; set; }
    public string? InvitationId { get; set; }

    public string Initial => string.IsNullOrEmpty(Name) ? (string.IsNullOrEmpty(Email) ? "?" : Email[..1].ToUpper()) : Name[..1].ToUpper();
}
