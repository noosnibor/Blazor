namespace PortalWeb.Models;

public class MonthModel
{
    public string Id { get; set; } = default!;
    public Dictionary<string, decimal> Amount { get; set; } = [];
}
