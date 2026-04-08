
namespace TDDByExample
{
    [Serializable]
    internal class RatePairChangeNotFoundException : Exception
    {
        public RatePairChangeNotFoundException()
        {
        }

        public RatePairChangeNotFoundException(string? message) : base(message)
        {
        }

        public RatePairChangeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}