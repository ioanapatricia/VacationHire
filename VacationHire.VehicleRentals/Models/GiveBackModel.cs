namespace VacationHire.VehicleRentals.Models;

public class GiveBackModel
{
    public string Currency { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }
    public int Days { get; set; }
    public string VehicleType { get; set; } 
    public string CustomerName { get; set; }    
    public string CustomerPhoneNumber { get; set; }
}       