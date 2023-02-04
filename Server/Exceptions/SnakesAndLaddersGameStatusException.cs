namespace Server.Exceptions
{
    public class SnakesAndLaddersGameStatusException : SnakesAndLaddersBaseException
    {
        public SnakesAndLaddersGameStatusException(string message) : base(message)
        {
        }

        public SnakesAndLaddersGameStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static SnakesAndLaddersGameStatusException GameAlreadyStartedException()
        {
            return new SnakesAndLaddersGameStatusException("Game already in progress.");
        }

        public static SnakesAndLaddersGameStatusException GameNotStartedException()
        {
            return new SnakesAndLaddersGameStatusException("Game has not started yet.");
        }

        public static SnakesAndLaddersGameStatusException GameIsFinishedException()
        {
            return new SnakesAndLaddersGameStatusException("Game has already ended.");
        }
    }
}
