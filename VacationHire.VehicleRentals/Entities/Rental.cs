namespace VacationHire.VehicleRentals.Entities;

public class Rental
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public VehicleType Type { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }

    public bool IsDamaged { get; set; }
    public bool IsTankEmpty { get; set; }       
    public bool IsReturned { get; set; }

    public decimal? Price { get; set; }
}