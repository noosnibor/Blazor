namespace PortalWeb.Models;

public class DailyReportModel
{
    public int flngCollectionKey { get; set; }
    public string? fstrFirstname { get; set; }
    public string? fstrLastname { get; set; }
    public string? fstrEmailAddress { get; set; }
    public DateTime fdtmTransactionDate { get; set; }
    public int flngCollectionNumber { get; set; }
    public int flngMemberStatusKey { get; set; }
    public int flngCollectionTypeKey { get; set; }
    public int flngPaymentTypeKey { get; set; }
    public decimal fcurAmount { get; set; }
    public decimal fcurLocalAmount { get; set; }
    public string? fstrCurrencyKey { get; set; }
    public bool blnActive { get; set; }
    public string? fstrLocationKey { get; set; }
    public string? fstrWho { get; set; }
    public DateTime fdtmWhen { get; set; }
}
