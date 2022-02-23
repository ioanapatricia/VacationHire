using AutoMapper;
using VacationHire.VehicleRentals.Dtos;
using VacationHire.VehicleRentals.Dtos.Customer;
using VacationHire.VehicleRentals.Dtos.Rental;
using VacationHire.VehicleRentals.Dtos.VehicleType;
using VacationHire.VehicleRentals.Entities;
using VacationHire.VehicleRentals.Models;

namespace VacationHire.VehicleRentals.Helpers;
public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        CreateMap<VehicleType, VehicleDto>();    
        CreateMap<Customer, CustomerForGetDto>();
        CreateMap<Rental, RentalForGetDto>();

        CreateMap<RentalForCreationDto, Rental>();
        CreateMap<GiveBackModel, GiveBackDto>();
        CreateMap<CustomerForCreationDto, Customer>();
    }
}
