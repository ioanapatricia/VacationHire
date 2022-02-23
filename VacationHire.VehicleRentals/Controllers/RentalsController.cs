using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VacationHire.VehicleRentals.Dtos;
using VacationHire.VehicleRentals.Dtos.Rental;
using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Models;
using VacationHire.VehicleRentals.ResultHandling;
using VacationHire.VehicleRentals.Services.Interfaces;

namespace VacationHire.VehicleRentals.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController : ControllerBase
{
    private readonly ILogger<RentalsController> _logger;
    private readonly IMapper _mapper;
    private readonly IRentalsService _rentalsService;
    private readonly ICustomersService _customersService;
    private readonly IVehicleTypesService _vehicleTypesService;

    public RentalsController(ILogger<RentalsController> logger, IMapper mapper, IRentalsService rentalsService, ICustomersService customersService, IVehicleTypesService vehicleTypesService)
    {
        _logger = logger;
        _mapper = mapper;
        _rentalsService = rentalsService;
        _customersService = customersService;
        _vehicleTypesService = vehicleTypesService;
    }

    [HttpGet("{id}", Name = "GetRental")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        _logger.LogInformation($"Started {nameof(GetAsync)} in API layer for id: {id}.");
        var rentalResult = await _rentalsService.GetAsync(id);
        var rental = rentalResult.Value;

        if (rental is null)
        {
            _logger.LogError($"Ended {nameof(GetAsync)} in API layer with 404 NotFound Result for id: {id}.");
            return NotFound();
        }

        var resultDto = _mapper.Map<RentalForGetDto>(rental);

        _logger.LogInformation($"Ended {nameof(GetAsync)} in API layer with 200 Ok Result for id: {id}.");
        return Ok(resultDto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllAsync()
    {
        _logger.LogInformation($"Started {nameof(GetAllAsync)} in API layer.");
        var rentalResult = await _rentalsService.GetAllAsync();
        var rentals = rentalResult.Value;

        if (!rentals.Any())
        {
            _logger.LogError($"Ended {nameof(GetAllAsync)} in API layer with 204 NoContent Result.");
            return NoContent();
        }

        var resultDto = _mapper.Map<List<RentalForGetDto>>(rentals);

        _logger.LogInformation($"Ended {nameof(GetAllAsync)} in API layer with 200 Ok Result.");
        return Ok(resultDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(RentalForCreationDto rentalForCreationDto)
    {
        _logger.LogInformation($"Started {nameof(CreateAsync)} in API layer with data: {JsonConvert.SerializeObject(rentalForCreationDto)}.");

        var customer = await _customersService.GetAsync((int)rentalForCreationDto.CustomerId);
        var vehicleType = await _vehicleTypesService.GetAsync((int)rentalForCreationDto.TypeId);

        if (customer is null || vehicleType is null)
        {
            _logger.LogError($"Ended {nameof(CreateAsync)} in API layer. Either customer or vehicleType was not found.");
            return (BadRequest("Either customer or vehicle type is not valid"));
        }

        var rental = _mapper.Map<Rental>(rentalForCreationDto);

        var rentalResult = await _rentalsService.CreateAsync(rental);

        if (rentalResult.IsFailure)
        {
            _logger.LogError($"Ended {nameof(CreateAsync)} in API layer. Error: {rentalResult.Error}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not create rental.");
        }

        var rentalForGetDto = _mapper.Map<RentalForGetDto>(rentalResult.Value);

        _logger.LogInformation($"Ended {nameof(CreateAsync)} in API layer.");
        return CreatedAtRoute("GetRental", new { id = rentalForGetDto.Id }, rentalForGetDto);
    }

    [HttpPost("{id}/giveBack")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReturnRentalAsync(int id, [FromBody] RentalForGivingBackDto rentalForGivingBackDto)
    {
        _logger.LogInformation($"Started {nameof(ReturnRentalAsync)} in API layer with value: {JsonConvert.SerializeObject(rentalForGivingBackDto)}.");
        var existingRentalResult = await _rentalsService.GetAsync(id);
        var existingRental = existingRentalResult.Value;

        if (existingRental is null)
        {
            _logger.LogError($"Ended {nameof(ReturnRentalAsync)} in API layer with 400 NotFound Result for id: {id}.");
            return NotFound($"This rental does not exist.");
        }

        if (existingRental.IsReturned)
        {
            _logger.LogError($"Ended {nameof(ReturnRentalAsync)} in API layer with 400 BadRequest Result for value: {JsonConvert.SerializeObject(rentalForGivingBackDto)}.");
            return BadRequest($"This rental has already been given back.");
        }

        Result<GiveBackModel> result = null;
        try
        {
            result = await _rentalsService.ReturnRentalAsync(existingRental, rentalForGivingBackDto);

            if (result.IsFailure)
            {
                _logger.LogError($"Ended {nameof(ReturnRentalAsync)} in API layer. Error: {result.Error}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Could not give back rental.");
            }

            var rentalResultDto = _mapper.Map<GiveBackDto>(result.Value);

            _logger.LogInformation($"Ended {nameof(GetAsync)} in API layer with value: {JsonConvert.SerializeObject(rentalResultDto)}.");
            return Ok(rentalResultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ended {nameof(ReturnRentalAsync)} in API layer. Error: {ex.Message}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not give back rental. External Conversion API is down, or currency does not exist.");
        }
    }
}