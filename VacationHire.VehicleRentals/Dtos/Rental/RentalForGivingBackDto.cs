using System.ComponentModel.DataAnnotations;

namespace VacationHire.VehicleRentals.Dtos.Rental;

public class RentalForGivingBackDto
{
    [Required]
    public DateTime? ActualEndDate { get; set; }

    public bool IsDamaged { get; set; }

    public bool IsTankEmpty { get; set; }

    // Should actually have a list of accepted currencies to validate against
    [MinLength(3)]
    [MaxLength(3)]
    public string DesiredCurrency { get; set; } 
}
