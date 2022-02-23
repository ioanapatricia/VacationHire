namespace VacationHire.VehicleRentals.Services.Interfaces;

public interface IConversionService
{
    Task<decimal> ConvertAmountFromUsdTo(string newCurrency, decimal price);
}
