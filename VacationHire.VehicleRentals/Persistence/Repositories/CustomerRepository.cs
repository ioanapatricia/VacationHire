using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Persistence.Repositories.Interfaces;

namespace VacationHire.VehicleRentals.Persistence.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly DataContext _context;
    public CustomerRepository(DataContext context)
        : base(context)
    {
        _context = context;
    }
}