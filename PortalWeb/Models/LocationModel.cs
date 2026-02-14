namespace PortalWeb.Models;

public class LocationModel
{
    public string?      fstrLocationKey     { get; set; }
    public string?      fstrLocationName    { get; set; }
    public string?      fstrLocationAddress { get; set; }
    public bool         fblnActive          { get; set; }
    public string?      fstrWho             { get; set; }
    public DateTime     fdtmWhen            { get; set; }
}
