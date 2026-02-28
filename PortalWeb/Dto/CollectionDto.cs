using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Dto;

public class CollectionDto
{
    public Guid          CollectionKey       { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "A first name is required")]
    public string?      Firstname           { get; set; }
    [Required(ErrorMessage = "A surname is required")]
    public string?      Lastname            { get; set; }
    [Required(ErrorMessage = "An email address is required"), 
     EmailAddress(ErrorMessage = "Email Address format is incorrect")]
    public string?      EmailAddress        { get; set; }
    public DateTime?    TransactionDate     { get; set; } = DateTime.Today;
    [Range(1, 100000000, ErrorMessage = "collection number must not be zero")]
    public int          CollectionNumber    { get; set; } = 1;
    [Range(1, 1000000000, ErrorMessage = "A member status is required")]
    public int          MemberStatusKey     { get; set; }
    [Range(1, 1000000000, ErrorMessage = "A collection type is required")]
    public int          CollectionTypeKey   { get; set; }
    [Range(1, 1000000000, ErrorMessage = "A payment type is required")]
    public int          PaymentTypeKey      { get; set; }
    [Range(1, 1000000000, ErrorMessage = "An amount is required")]
    public decimal      Amount              { get; set; }
    public decimal      LocalAmount         { get; set; } = 0;
    [Required(ErrorMessage = "A currency type is required")]
    public string?      CurrencyKey         { get; set; }
    public bool         Active              { get; set; } = true;
    public string?      LocationKey         { get; set; }
    public string?      Who                 { get; set; } = "system";
    [Required(ErrorMessage = "A collection date is required")]
    public DateTime?    When                { get; set; } = DateTime.Today;
}
