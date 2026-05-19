using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventName), "EventName")]
public partial class EventTeamViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string InviteEmail { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string SelectedRole { get; set; } = "Credenciamento";

    public ObservableCollection<TeamMember> PendingInvites { get; } = new();
    public ObservableCollection<TeamMember> ActiveTeam { get; } = new();

    public int PendingCount => PendingInvites.Count;
    public int ActiveCount => ActiveTeam.Count;

    public EventTeamViewModel()
    {
        LoadMockData();
    }

    private void LoadMockData()
    {
        PendingInvites.Add(new TeamMember { Email = "marina@email.com", Role = "Credenciamento", IsPending = true });

        ActiveTeam.Add(new TeamMember { Name = "Lucas Alencar", Email = "lucas@email.com", Role = "Credenciamento" });
        ActiveTeam.Add(new TeamMember { Name = "Ana Beatriz", Email = "ana@email.com", Role = "Organização" });
    }

    [RelayCommand]
    private void ToggleRole(string role)
    {
        SelectedRole = role;
    }

    [RelayCommand]
    private async Task InviteAsync()
    {
        if (string.IsNullOrWhiteSpace(InviteEmail)) return;

        PendingInvites.Add(new TeamMember
        {
            Email = InviteEmail.Trim(),
            Role = SelectedRole,
            IsPending = true
        });

        InviteEmail = string.Empty;
        OnPropertyChanged(nameof(PendingCount));
        await Task.CompletedTask;
    }

    [RelayCommand]
    private void RemoveMember(TeamMember member)
    {
        if (member.IsPending)
        {
            PendingInvites.Remove(member);
            OnPropertyChanged(nameof(PendingCount));
        }
        else
        {
            ActiveTeam.Remove(member);
            OnPropertyChanged(nameof(ActiveCount));
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

    public string Initial => string.IsNullOrEmpty(Name) ? "?" : Name[..1].ToUpper();
}
