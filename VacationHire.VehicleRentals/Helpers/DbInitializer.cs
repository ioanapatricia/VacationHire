using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Persistence;

namespace VacationHire.VehicleRentals.Helpers;


public class DbInitializer
{
    private readonly DataContext _context;

    public DbInitializer(DataContext context)
    {
        _context = context;
    }

    public async Task MigrateAndSeed()
    {
        await _context.Database.MigrateAsync();

        if (_context.Rentals.Any())
            return;

        // Vehicle Types
        var vehicleTypeData = File.ReadAllText("./Helpers/DataForSeed/VehicleTypes.json");
        var vehicleTypes = JsonConvert.DeserializeObject<List<VehicleType>>(vehicleTypeData);

        foreach (var vehicleType in vehicleTypes)
            _context.Add(vehicleType);

        // Customers
        var customerData = File.ReadAllText("./Helpers/DataForSeed/Customers.json");
        var customers = JsonConvert.DeserializeObject<List<Customer>>(customerData);

        foreach (var customer in customers)
            _context.Add(customer);

        _context.SaveChanges();

        // Rentals
        var rentalData = File.ReadAllText("./Helpers/DataForSeed/Rentals.json");
        var rentals = JsonConvert.DeserializeObject<List<Rental>>(rentalData);

        foreach (var rental in rentals)
            _context.Add(rental);

        _context.SaveChanges();
    }
}
