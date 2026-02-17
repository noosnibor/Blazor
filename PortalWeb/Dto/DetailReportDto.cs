using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Dto;

public class DetailReportDto : IValidatableObject
{
    public string? LocationKey { get; set; }
    [Required(ErrorMessage = "This field is required")] public DateTime? From { get; set; }
    [Required(ErrorMessage = "This field is required")] public DateTime? To { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public int? CollectionTypeKey { get; set; }
    public int? PaymentTypeKey { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (From.HasValue && To.HasValue)
        {
            if (To < From)
            {
                yield return new ValidationResult(
                    "Date To cannot be earlier than Date From",
                    [nameof(To)]
                );
            }

            if (To > From.Value.AddYears(1))
            {
                yield return new ValidationResult(
                    "Date To cannot exceed one year from Date From",
                    [nameof(To)]
                );
            }
        }
    }
}
