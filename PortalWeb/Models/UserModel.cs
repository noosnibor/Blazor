namespace PortalWeb.Models;

public class UserModel
{
    public Guid flngUserKey { get; set; }
    public string? fstrUsername { get; set; }
    public string? fstrPassword { get; set; }
    public string? fstrFirstname { get; set; }
    public string? fstrLastname { get; set; }
    public string? fstrEmailAddress { get; set; }
    public string? fstrLocationKey { get; set; }
    public int flngRoleKey { get; set; }
    public string? fstrCurrencyKey { get; set; }
    public bool fblnActive { get; set; }
    public bool fblnPasswordChanged { get; set; }
    public DateTime fdtmStart { get; set; }
    public DateTime fdtmEnd { get; set; }
    public string? fstrWho { get; set; }
    public DateTime fdtmWhen { get; set; }
}
