using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Persistence;
using VacationHire.VehicleRentals.ResultHandling;
using VacationHire.VehicleRentals.Services.Interfaces;

namespace VacationHire.VehicleRentals.Services;
public class VehicleTypesService : IVehicleTypesService
{
    private readonly ILogger<VehicleTypesService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public VehicleTypesService(ILogger<VehicleTypesService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<VehicleType>> GetAsync(int id)
    {
        _logger.LogInformation($"Started {nameof(GetAsync)} in Service Layer for id: {id}.");

        var vehicleType = await _unitOfWork.VehicleTypes.GetAsync(id);

        _logger.LogInformation($"Ended {nameof(GetAsync)} in Service Layer for id: {id}.");

        return Result<VehicleType>.Ok(vehicleType);
    }
}