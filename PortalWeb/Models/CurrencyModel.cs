namespace PortalWeb.Models;

public class CurrencyModel
{
    public  string?     fstrCurrencyKey     { get; set; }
    public  string?     fstrCurrencyType    { get; set; }
    public  string?     fstrLocationKey     { get; set; }
    public  decimal     fcurAmount          { get; set; }
    public  bool        fblnActive          { get; set; }
    public  string?     fstrWho             { get; set; }
    public  DateTime    fdtmEffectiveFrom   { get; set; }
    public  DateTime    fdtmEffectiveTo     { get; set; }
}
