using VacationHire.VehicleRentals.Dtos.Rental;
using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Models;
using VacationHire.VehicleRentals.ResultHandling;

namespace VacationHire.VehicleRentals.Services.Interfaces;

public interface IRentalsService
{
    Task<Result<IEnumerable<Rental>>> GetAllAsync();
    Task<Result<Rental>> GetAsync(int id);
    Task<Result<Rental>> CreateAsync(Rental rental);
    Task<Result<GiveBackModel>> ReturnRentalAsync(Rental rental, RentalForGivingBackDto rentalForGivingBackDto);
}
    