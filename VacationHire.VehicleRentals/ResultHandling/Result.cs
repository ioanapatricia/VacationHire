namespace VacationHire.VehicleRentals.ResultHandling;

public class Result
{
    public bool Success { get; private set; }
    public bool IsFailure => !Success;
    public string Error { get; private set; }

    public static Result Ok() =>
        new()
        {
            Success = true,
        };

    public static Result Fail(string errorMessage) =>
        new()
        {
            Success = false,
            Error = errorMessage
        };
}

public class Result<T>
{
    public bool Success { get; private set; }
    public bool IsFailure => !Success;
    public string Error { get; private set; }
    public T Value { get; set; }


    public static Result<T> Ok(T value) =>
        new()
        {
            Success = true,
            Value = value
        };

    public static Result<T> Fail(string errorMessage) =>
        new()
        {
            Success = false,
            Error = errorMessage
        };
}