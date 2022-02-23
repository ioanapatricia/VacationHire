using VacationHire.VehicleRentals.Persistence.Repositories.Interfaces;
using VacationHire.VehicleRentals.ResultHandling;

namespace VacationHire.VehicleRentals.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    public ICustomerRepository Customers { get; set; }
    public IRentalRepository Rentals { get; set; }
    public IVehicleTypeRepository VehicleTypes { get; set; }

    public UnitOfWork(DataContext context, ICustomerRepository customers, IRentalRepository rentals, IVehicleTypeRepository vehicleTypes)
    {
        _context = context;
        Customers = customers;
        Rentals = rentals;
        VehicleTypes = vehicleTypes;
    }

    public async Task<Result> CompleteAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail($"Error Message: {e.Message}, Inner Exception: {e.InnerException}");
        }
    }
}
