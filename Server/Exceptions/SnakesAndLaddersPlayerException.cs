namespace Server.Exceptions
{
    public class SnakesAndLaddersPlayerException : SnakesAndLaddersBaseException
    {
        public SnakesAndLaddersPlayerException(string message) : base(message)
        {
        }

        public SnakesAndLaddersPlayerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static SnakesAndLaddersPlayerException PlayerExistsAlreadyException(string name)
        {
            return new SnakesAndLaddersPlayerException($"The Player name {name} existst already.");
        }

        public static SnakesAndLaddersPlayerException NotEnoughPlayersException()
        {
            return new SnakesAndLaddersPlayerException($"There are not enough Players to create a new Game. Minimum number of players is {Constants.MINIMUM_NUMBER_OF_PLAYERS_BY_GAME}.");
        }

        public static SnakesAndLaddersPlayerException PlayerNotFoundException(string name)
        {
            return new SnakesAndLaddersPlayerException($"The player {name} could not be found.");
        }

        public static SnakesAndLaddersPlayerException NoPlayersFound()
        {
            return new SnakesAndLaddersPlayerException($"No Players were found");
        }

        public static SnakesAndLaddersPlayerException PlayerHasNoTurnToMove(string name)
        {
            throw new SnakesAndLaddersPlayerException($"The player {name} has no Turn to be moved.");
        }
    }
}
