namespace Servindustria.Models;

public class User {
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string? Company { get; set; } = String.Empty;
    public string? Address { get; set; } = String.Empty;
    public int? Nif { get; set; } = 0;
    public int? Phone { get; set; } = 0;
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public bool IsAdmin { get; set; } = false;
}