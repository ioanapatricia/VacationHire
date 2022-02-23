using Microsoft.EntityFrameworkCore;
using VacationHire.VehicleRentals.Helpers;
using VacationHire.VehicleRentals.Models.Configuration;
using VacationHire.VehicleRentals.Persistence;
using VacationHire.VehicleRentals.Persistence.Repositories;
using VacationHire.VehicleRentals.Persistence.Repositories.Interfaces;
using VacationHire.VehicleRentals.Services;
using VacationHire.VehicleRentals.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging();

builder.Services.Configure<CurrencyLayerConfiguration>(builder.Configuration.GetSection("CurrencyLayerConfiguration"));

builder.Services.AddDbContextPool<DataContext>(options =>
        options.UseSqlServer
        (
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();

builder.Services.AddScoped<DbInitializer>();

builder.Services.AddScoped<IRentalsService, RentalsService>();
builder.Services.AddScoped<IVehicleTypesService, VehicleTypesService>();
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IConversionService, ConversionService>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
