using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Dto;

public class DailyReportDto
{
    public string? LocationKey { get; set; } = null;
    [Required(ErrorMessage = "This field is required")] public DateTime? TransactionDateFrom { get; set; }
    public int? CollectionNumber { get; set; } = null;
}
