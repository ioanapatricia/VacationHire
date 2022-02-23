using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VacationHire.VehicleRentals.Dtos.Customer;
using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Services.Interfaces;

namespace VacationHire.VehicleRentals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<RentalsController> _logger;
        private readonly IMapper _mapper;
        private readonly ICustomersService _customersService;

        public CustomersController(ILogger<RentalsController> logger, IMapper mapper, ICustomersService customersService)
        {
            _logger = logger;
            _mapper = mapper;
            _customersService = customersService;
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            _logger.LogInformation($"Started {nameof(GetAsync)} in API layer for id: {id}.");
            var customerResult = await _customersService.GetAsync(id);
            var customer = customerResult.Value;

            if (customer is null)
            {
                _logger.LogError($"Ended {nameof(GetAsync)} in API layer with 404 NotFound Result for id: {id}.");
                return NotFound();
            }

            var resultDto = _mapper.Map<CustomerForGetDto>(customer);

            _logger.LogInformation($"Ended {nameof(GetAsync)} in API layer with 200 Ok Result for id: {id}.");
            return Ok(resultDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync(CustomerForCreationDto customerForCreationDto)
        {
            _logger.LogInformation($"Started {nameof(CreateAsync)} in API layer with data: {JsonConvert.SerializeObject(customerForCreationDto)}.");

            var customer = _mapper.Map<Customer>(customerForCreationDto);

            var customerResult = await _customersService.CreateAsync(customer);

            if (customerResult.IsFailure)
            {
                _logger.LogError($"Ended {nameof(CreateAsync)} in API layer. Error: {customerResult.Error}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Could not create customer.");
            }

            var customerForGetDto = _mapper.Map<CustomerForGetDto>(customerResult.Value);

            _logger.LogInformation($"Ended {nameof(CreateAsync)} in API layer.");
            return CreatedAtRoute("GetCustomer", new { id = customerForGetDto.Id }, customerForGetDto);
        }
    }
}
