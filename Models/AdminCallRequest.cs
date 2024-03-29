namespace Servindustria.Models;

public class AdminCallRequest {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Company { get; set; }
    public int Phone { get; set; }
    public string? Message { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public bool Seen { get; set; } = false;
}