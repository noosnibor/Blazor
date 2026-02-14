using System.ComponentModel.DataAnnotations;

namespace PortalWeb.Dto;

public class LocationDto
{
    [Required(ErrorMessage = "Campus Code is required"), MinLength(2, ErrorMessage = "Minimum length must be 2 characters"),]
    public string?  LocationKey     { get; set; }

    [Required(ErrorMessage = "Campus Description is required")]
    public string?  LocationName    { get; set; }

    [Required(ErrorMessage = "Campus Address is required")]
    public string?  LocationAddress { get; set; }
    public bool     Active          { get; set; }
    public string?  Who { get; set; } = "system";
    public DateTime? When { get; set; } = DateTime.Now;
 
}
