using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Persistence.Repositories.Interfaces;

namespace VacationHire.VehicleRentals.Persistence.Repositories;

public class VehicleTypeRepository : Repository<VehicleType>, IVehicleTypeRepository
{
    private readonly DataContext _context;
    public VehicleTypeRepository(DataContext context)
        : base(context)
    {
        _context = context;
    }
}