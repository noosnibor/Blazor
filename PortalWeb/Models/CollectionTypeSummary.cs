namespace PortalWeb.Models;

public class CollectionTypeSummary
{
    public List<string> Currencies { get; set; } = [];
    public List<int> CollectionTypes { get; set; } = [];
    public Dictionary<(int, string), decimal> CellTotals { get; set; } = [];
    public Dictionary<int, decimal> RowTotals { get; set; } = [];
    public Dictionary<string, decimal> ColumnTotals { get; set; } = [];
    public decimal GrandTotal { get; set; } = 0;
}
