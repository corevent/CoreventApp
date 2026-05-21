namespace CoreventApp.Models;

public class CollaboratorEvent
{
    public string ImageUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string RoleColor { get; set; } = string.Empty;
    public string RoleTextColor { get; set; } = string.Empty;
    public bool HasActionButton { get; set; }
    public int ParticipantCount { get; set; }
}
