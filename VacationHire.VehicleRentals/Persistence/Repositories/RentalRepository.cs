using Microsoft.EntityFrameworkCore;
using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Persistence.Repositories.Interfaces;

namespace VacationHire.VehicleRentals.Persistence.Repositories;

public class RentalRepository : Repository<Rental>, IRentalRepository
{
    private readonly DataContext _context;
    public RentalRepository(DataContext context)
        : base(context)
    {
        _context = context;
    }

    public override async Task<Rental> GetAsync(int id)
    {
        return await _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Type)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public override async Task<IEnumerable<Rental>> GetAllAsync()
    {
        return await _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Type)
            .ToListAsync();
    }
}   