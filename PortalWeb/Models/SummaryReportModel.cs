namespace PortalWeb.Models;

public class SummaryReportModel
{
    public int flngPaymentTypeKey { get; set; }
    public int flngCollectionTypeKey { get; set; }
    public string? fstrCurrencyKey { get; set; }
    public string? fstrLocationKey { get; set; }
    public string? fstrMonth { get; set; }
    public DateTime fdtmTransactionDate { get; set; }
    public decimal fcurAmount { get; set; }
    public decimal fcurLocalAmount { get; set; }
}
