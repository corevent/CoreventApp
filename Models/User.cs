namespace CoreventApp.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;
    public string Cellphone { get; set; } = string.Empty; // Atributo temporário
    public string AvatarUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}