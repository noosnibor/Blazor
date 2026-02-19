namespace PortalWeb.Models;

public class DetailReportModel
{
    public string? fstrFullname { get; set; }
    public int flngMemberStatusKey { get; set; }
    public int flngCollectionTypeKey { get; set; }
    public int flngPaymentTypeKey { get; set; }
    public string? fstrCurrencyKey { get; set; }
    public decimal fcurAmount { get; set; }
    public decimal fcurLocalAmount { get; set; }
}
