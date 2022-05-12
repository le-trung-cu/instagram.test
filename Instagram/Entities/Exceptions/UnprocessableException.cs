namespace Entities.Exceptions
{
    public abstract class UnprocessableException : Exception
    {
        public UnprocessableException(string message) : base(message)
        {
        }
    }
}