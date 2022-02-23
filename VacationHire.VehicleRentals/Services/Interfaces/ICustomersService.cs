using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.ResultHandling;

namespace VacationHire.VehicleRentals.Services.Interfaces;
public interface ICustomersService
{
    Task<Result<Customer>> GetAsync(int id);
    Task<Result<Customer>> CreateAsync(Customer customer);
}
