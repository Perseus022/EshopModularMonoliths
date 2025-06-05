namespace Shared.Exceptions;

public class BadRequestExcepton : Exception
{
    public BadRequestExcepton(string message) :
        base(message)
    {
    }
    public BadRequestExcepton(string message, object details) : base(message)
    {
        Details = details;
    }
    public object? Details { get; } = null;
}
