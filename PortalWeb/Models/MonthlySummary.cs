namespace PortalWeb.Models;

public class MonthlySummary
{
    public IReadOnlyList<string> Months { get; init; } = [];
    public IReadOnlyList<MonthModel> Data { get; init; } = [];
}
