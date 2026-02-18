using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Dto;

public class SummaryReportDto : IValidatableObject
{
    public string? LocationKey { get; set; } = null;
    [Required(ErrorMessage = "This field is required")] public DateTime? TransactionDateFrom { get; set; }
    [Required(ErrorMessage = "This field is required")] public DateTime? TransactionDateTo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TransactionDateFrom.HasValue && TransactionDateTo.HasValue)
        {
            if (TransactionDateTo < TransactionDateFrom)
            {
                yield return new ValidationResult(
                    "Date To cannot be earlier than Date From",
                    [nameof(TransactionDateTo)]
                );
            }

            if (TransactionDateTo > TransactionDateFrom.Value.AddYears(1))
            {
                yield return new ValidationResult(
                    "Date To cannot exceed one year from Date From",
                    [nameof(TransactionDateTo)]
                );
            }
        }
    }
}
