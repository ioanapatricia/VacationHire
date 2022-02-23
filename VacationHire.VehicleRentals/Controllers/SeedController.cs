using Microsoft.AspNetCore.Mvc;
using VacationHire.VehicleRentals.Helpers;

namespace VacationHire.VehicleRentals.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly DbInitializer _dbInitializer;

    public SeedController(ILogger<SeedController> logger, IWebHostEnvironment hostEnvironment, DbInitializer dbInitializer)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        _dbInitializer = dbInitializer;
    }

    [HttpPost]
    public async Task<IActionResult> SeedData()
    {
        if (!_hostEnvironment.IsDevelopment())
            return StatusCode(403);

        try
        {
            _logger.LogInformation("Seeding database...");

            await _dbInitializer.MigrateAndSeed();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred during migration. Exception: {ex.Message}{Environment.NewLine}" +
               $"Inner exception: {ex.InnerException}{Environment.NewLine}" +
               $"Source: {ex.Source}{Environment.NewLine}" +
               $"Stacktrace: {ex.StackTrace}{Environment.NewLine}");

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
        }

        return Ok("Database migrated successfully!");
    }
}