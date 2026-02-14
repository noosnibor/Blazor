using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Dto;

public class CurrencyDto
{
    [Required(ErrorMessage = "Campus Code is required"),
    MinLength(3, ErrorMessage = "Minimum length must be 3 characters")]
    public string?      CurrencyKey     { get; set; }
    [Required(ErrorMessage = "Currency Type is required")]
    public string?      CurrencyType    { get; set; }
    [Required(ErrorMessage = "An location is required")]
    public string?      LocationKey     { get; set; }
    [Required(ErrorMessage = "An amount is required")]
    [Range(1, 1000000000, ErrorMessage = "Amount must not be zero")]
    public decimal      Amount          { get; set; }
    public bool         Active          { get; set; } = true;
    public string?      Who             { get; set; } = "system";
    public DateTime?    EffectiveFrom   { get; set; } = DateTime.Today;
    [Required(ErrorMessage = "An effective date to is required")]
    public DateTime?    EffectiveTo     { get; set; } = null;
}
