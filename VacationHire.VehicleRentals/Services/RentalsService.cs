using Newtonsoft.Json;
using VacationHire.VehicleRentals.Dtos.Rental;
using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Models;
using VacationHire.VehicleRentals.Persistence;
using VacationHire.VehicleRentals.ResultHandling;
using VacationHire.VehicleRentals.Services.Interfaces;

namespace VacationHire.VehicleRentals.Services;

public class RentalsService : IRentalsService   
{
    private readonly ILogger<RentalsService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConversionService _convertCurrenciesService;

    public RentalsService(ILogger<RentalsService> logger, IUnitOfWork unitOfWork, IConversionService convertCurrenciesService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _convertCurrenciesService = convertCurrenciesService;
    }

    public async Task<Result<Rental>> GetAsync(int id)
    {
        _logger.LogInformation($"Started {nameof(GetAsync)} in Service Layer for id: {id}.");

        var rental = await _unitOfWork.Rentals.GetAsync(id);

        _logger.LogInformation($"Ended {nameof(GetAsync)} in Service Layer for id: {id}.");

        return Result<Rental>.Ok(rental);
    }

    public async Task<Result<IEnumerable<Rental>>> GetAllAsync()
    {
        _logger.LogInformation($"Started {nameof(GetAllAsync)} in Service Layer");

        var rentals = await _unitOfWork.Rentals.GetAllAsync();

        _logger.LogInformation($"Ended {nameof(GetAllAsync)} in Service Layer.");

        return Result<IEnumerable<Rental>>.Ok(rentals);
    }

    public async Task<Result<Rental>> CreateAsync(Rental rental)
    {
        _logger.LogInformation($"Started {nameof(CreateAsync)} in Service Layer with data: {JsonConvert.SerializeObject(rental)}");
        _unitOfWork.Rentals.Add(rental);

        var result = await _unitOfWork.CompleteAsync();

        if (result.Success)
        {
            _logger.LogInformation($"Ended {nameof(CreateAsync)} in Service Layer");
            return Result<Rental>.Ok(rental);
        }

        _logger.LogError($"Ended {nameof(CreateAsync)} in Service Layer with error: {result.Error}");
        return Result<Rental>.Fail(result.Error);
    }

    public async Task<Result<GiveBackModel>> ReturnRentalAsync(Rental rental, RentalForGivingBackDto rentalForGivingBackDto)
    {
        var price = GetPrice(rental, rentalForGivingBackDto);

        UpdateRentalWithGiveBack(rental, rentalForGivingBackDto, price);

        var result = await _unitOfWork.CompleteAsync();

        if (result.IsFailure)
            return Result<GiveBackModel>.Fail(result.Error);

        var convertedPrice = await _convertCurrenciesService.ConvertAmountFromUsdTo(rentalForGivingBackDto.DesiredCurrency, price);

        convertedPrice = Math.Round(convertedPrice, 2, MidpointRounding.ToZero);

        var giveBack = GetGiveBackModel(rental, rentalForGivingBackDto, convertedPrice);

        return Result<GiveBackModel>.Ok(giveBack);
    }

    private static decimal GetPrice(Rental rental, RentalForGivingBackDto rentalForGivingBackDto)
    {
        int days = GetRentalDays(rental.StartDate, (DateTime)rentalForGivingBackDto.ActualEndDate);

        decimal price = rental.Type.PricePerDay * days;

        if (rentalForGivingBackDto.IsDamaged)
            price += 200;

        if (rentalForGivingBackDto.IsTankEmpty)
            price += 100;

        return price;
    }

    private static void UpdateRentalWithGiveBack(Rental rental, RentalForGivingBackDto rentalForGivingBackDto, decimal price)
    {
        rental.ActualEndDate = rentalForGivingBackDto.ActualEndDate;
        rental.IsDamaged = rentalForGivingBackDto.IsDamaged;
        rental.IsTankEmpty = rentalForGivingBackDto.IsTankEmpty;
        rental.IsReturned = true;
        rental.Price = price;
    }

    private static GiveBackModel GetGiveBackModel(Rental rental, RentalForGivingBackDto rentalForGivingBackDto, decimal convertedPrice)
    {
         return new GiveBackModel()
        {
            Currency = rentalForGivingBackDto.DesiredCurrency.ToUpper(),
            Price = convertedPrice,
            StartDate = rental.StartDate,
            EndDate = (DateTime)rentalForGivingBackDto.ActualEndDate,
            Days = GetRentalDays(rental.StartDate, (DateTime)rental.ActualEndDate),
            VehicleType = rental.Type.Name,
            CustomerName = rental.Customer.Name,
            CustomerPhoneNumber = rental.Customer.PhoneNumber
        };
    }   

    private static int GetRentalDays(DateTime startDate, DateTime endDate)
    {
        return (int)(endDate - startDate).TotalDays;
    }
}