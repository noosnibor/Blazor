using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Dto;

public class CurrencyDto : IValidatableObject
{
    [Required(ErrorMessage = "Campus Code is required"),
    MinLength(3, ErrorMessage = "Minimum length must be 3 characters")]
    public string?      CurrencyKey     { get; set; }
    [Required(ErrorMessage = "Currency Type is required")]
    public string?      CurrencyType    { get; set; }
    [Required(ErrorMessage = "A location is required")]
    public string?      LocationKey     { get; set; }
    [Required(ErrorMessage = "An amount is required")]
    [Range(1, 1000000000, ErrorMessage = "Amount must not be zero")]
    public decimal      Amount          { get; set; }
    public bool         Active          { get; set; } = true;
    public string?      Who             { get; set; } = "system";
    [Required(ErrorMessage = "An effective from to is required")]
    public DateTime?    EffectiveFrom   { get; set; } = DateTime.Today;
    [Required(ErrorMessage = "An effective date to is required")]
    public DateTime?    EffectiveTo     { get; set; } = null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EffectiveFrom.HasValue && EffectiveTo.HasValue)
        {
            if (EffectiveTo.Value < EffectiveFrom.Value)
            {
                yield return new ValidationResult(
                    "Date To cannot be earlier than Date From",
                    [nameof(EffectiveTo)]
                );
            }

            if (EffectiveTo.Value > EffectiveFrom.Value.AddYears(1))
            {
                yield return new ValidationResult(
                    "Date To cannot exceed one year from Date From",
                    [nameof(EffectiveTo)]
                );
            }
        }
    }
}
