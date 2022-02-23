using VacationHire.VehicleRentals.Dtos.Customer;
using VacationHire.VehicleRentals.Dtos.VehicleType;

namespace VacationHire.VehicleRentals.Dtos.Rental;

public class RentalForGetDto
{
    public int Id { get; set; }
    public VehicleDto Type { get; set; }
    public CustomerForGetDto Customer { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }    
    public DateTime? ActualEndDate { get; set; }

    public bool IsDamaged { get; set; } 
    public bool IsTankEmpty { get; set; }
    public bool IsReturned { get; set; }

    public decimal? Price { get; set; }
}