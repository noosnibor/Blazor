using System.ComponentModel.DataAnnotations;
using PortalWeb.Services;

namespace PortalWeb.Dto;

public class UserDto
{
    public Guid flngUserKey { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Username field is required")]
    public string fstrUsername { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password field is required")]
    public string fstrPassword { get; set; } = MemoryStoredData.DefaultPassword;

    [Required(ErrorMessage = "Firstname field is required")]
    public string fstrFirstname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lastname field is required")]
    public string fstrLastname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email Address field is required"), EmailAddress(ErrorMessage = "Invalid email format")]
    public string fstrEmailAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "A campus is required")]
    public string fstrLocationKey { get; set; } = string.Empty;
    [Required(ErrorMessage = "A role is required")]
    public int? flngRoleKey { get; set; }
    [Required(ErrorMessage = "A currency is required")]
    public string? fstrCurrencyKey { get; set; } = string.Empty;
    public bool fblnActive { get; set; } = true;
    public bool fblnPasswordChanged { get; set; } = false;
    [Required(ErrorMessage = "A start date is required")]
    public DateTime? fdtmStart { get; set; } = null;
    [Required(ErrorMessage = "A end date is required")]
    public DateTime? fdtmEnd { get; set; } = null;
    public string fstrWho { get; set; } = "Batch";
    public DateTime fdtmWhen { get; set; } = DateTime.Now;
}
