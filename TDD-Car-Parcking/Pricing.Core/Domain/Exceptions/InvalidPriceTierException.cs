namespace Pricing.Core.Tests.Domain.Exceptions;

public class InvalidPriceTierException : Exception
{
    string _message { get; set; }

    public InvalidPriceTierException(string message)
    {
        _message = message;
    }
    
    public override string Message => _message;
}