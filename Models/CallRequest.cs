namespace Servindustria.Models;

public class CallRequest {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Company { get; set; }
    public int Phone { get; set; }
    public string? Message { get; set; }
}