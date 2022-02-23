using VacationHire.VehicleRentals.Persistence.Repositories.Interfaces;
using VacationHire.VehicleRentals.ResultHandling;

namespace VacationHire.VehicleRentals.Persistence;

public interface IUnitOfWork
{
    ICustomerRepository Customers { get; }
    IRentalRepository Rentals { get; }  
    IVehicleTypeRepository VehicleTypes { get; }

    Task<Result> CompleteAsync();
}   