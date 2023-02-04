namespace Server.Exceptions
{
    public class SnakesAndLaddersBoardException : SnakesAndLaddersBaseException
    {
        public SnakesAndLaddersBoardException(string message) : base(message)
        {
        }

        public SnakesAndLaddersBoardException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static SnakesAndLaddersBoardException BoardNotLoadedException()
        {
            return new SnakesAndLaddersBoardException("Board unloaded.");
        }

        public static SnakesAndLaddersBoardException BoardPositionNotExists()
        {
            return new SnakesAndLaddersBoardException("SquareID in the Board not found.");
        }

    }
}
