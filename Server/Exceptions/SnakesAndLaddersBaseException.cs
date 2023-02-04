namespace Server.Exceptions
{
    public class SnakesAndLaddersBaseException : Exception
    {
        public SnakesAndLaddersBaseException(string message) : base(message)
        {
        }

        public SnakesAndLaddersBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
