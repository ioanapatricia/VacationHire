using Newtonsoft.Json;
using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Persistence;
using VacationHire.VehicleRentals.ResultHandling;
using VacationHire.VehicleRentals.Services.Interfaces;

namespace VacationHire.VehicleRentals.Services;
public class CustomersService : ICustomersService
{
    private readonly ILogger<VehicleTypesService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CustomersService(ILogger<VehicleTypesService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Customer>> GetAsync(int id)
    {
        _logger.LogInformation($"Started {nameof(GetAsync)} in Service Layer for id: {id}.");

        var customer = await _unitOfWork.Customers.GetAsync(id);

        _logger.LogInformation($"Ended {nameof(GetAsync)} in Service Layer for id: {id}.");

        return Result<Customer>.Ok(customer);
    }

    public async Task<Result<Customer>> CreateAsync(Customer customer)
    {
        _logger.LogInformation($"Started {nameof(CreateAsync)} in Service Layer with data: {JsonConvert.SerializeObject(customer)}");
        _unitOfWork.Customers.Add(customer);

        var result = await _unitOfWork.CompleteAsync();

        if (result.Success)
        {
            _logger.LogInformation($"Ended {nameof(CreateAsync)} in Service Layer");
            return Result<Customer>.Ok(customer);
        }

        _logger.LogError($"Ended {nameof(CreateAsync)} in Service Layer with error: {result.Error}");
        return Result<Customer>.Fail(result.Error);
    }
}