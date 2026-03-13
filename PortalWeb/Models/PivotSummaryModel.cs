namespace PortalWeb.Models;

public class PivotSummaryModel
{
    public List<string> Columns { get; set; } = [];
    public List<string> Rows { get; set; } = [];

    public Dictionary<(string Row, string Column), decimal> CellTotals { get; set; } = [];
    public Dictionary<(string Row, string Column), decimal> CellTotalsUSD { get; set; } = [];
    public Dictionary<string, decimal> RowTotalsUSD { get; set; } = [];
    public Dictionary<string, decimal> ColumnTotalsUSD { get; set; } = [];


    public Dictionary<string, decimal> RowTotals { get; set; } = [];
    public Dictionary<string, decimal> ColumnTotals { get; set; } = [];

    public decimal GrandTotal { get; set; }
    public decimal GrandTotalUSD { get; set; }

}
