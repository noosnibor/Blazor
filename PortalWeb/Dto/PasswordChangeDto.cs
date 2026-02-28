using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Dto;

public class PasswordChangeDto
{
    [Required(ErrorMessage = "Password field is required")]
    public string? Password { get; set; }
    [Required(ErrorMessage = "Password Confirmed field is required")]
    [Compare("Password", ErrorMessage = "Password does not match")]
    public string? ConfirmPassword { get; set; }
}
