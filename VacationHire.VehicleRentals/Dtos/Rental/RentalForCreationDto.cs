using System.ComponentModel.DataAnnotations;

namespace VacationHire.VehicleRentals.Dtos.Rental;

public class RentalForCreationDto
{
    [Required]
    public int? TypeId { get; set; }

    [Required]
    public int? CustomerId { get; set; }

    [Required]
    public DateTime? StartDate { get; set; }

    [Required]  
    public DateTime? PlannedEndDate { get; set; }
}