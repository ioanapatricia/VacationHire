using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.ResultHandling;

namespace VacationHire.VehicleRentals.Services.Interfaces;
public interface IVehicleTypesService
{
    Task<Result<VehicleType>> GetAsync(int id);
}
