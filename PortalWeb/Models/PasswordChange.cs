using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Models;

public class PasswordChange
{
    [Required(ErrorMessage = "Password field is required")]
    public string? Password { get; set; }
    [Required(ErrorMessage = "Password Confirmed field is required")]
    [Compare("fstrPassword", ErrorMessage = "Password does not match")]
    public string? ConfirmPassword { get; set; }
}
