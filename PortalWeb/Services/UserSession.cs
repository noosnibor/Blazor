namespace PortalWeb.Services;

public record UserSession
{
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    public string? LocationKey { get; set; } = "CAN";
    public string? Address { get; set; }
    public string? CurrenyKey { get; set; } = "CAN";
    public string? CurrencyType { get; set; }
    public bool IsAuthenticated { get; set; }
}
