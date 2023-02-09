using Server.Domain;

namespace Server.Services
{
    public class GameService : IGameService
    {
        IDiceRoller _diceRoller;
        IGameFactory _gameFactory;
        private bool _isWon = false;
        private int _position = 0;
        private static Dictionary<string, List<int>> _positionHistory = new Dictionary<string, List<int>>();

        public GameService(IDiceRoller diceRoller, IGameFactory gameFactory)
        {
            _diceRoller = diceRoller;
            _gameFactory = gameFactory;
        }

        public GameMatrix Move(string userName, int diceValue)
        {
            if (_positionHistory.GetValueOrDefault(userName)?.Sum() <= 100)
            {
                _positionHistory.GetValueOrDefault(userName)?.Add(diceValue);
                _position = _positionHistory.GetValueOrDefault(userName)!.Sum();
            }

            _isWon = (_position >= 100);

            var gameMatrix = new GameMatrix()
            {
                Position = _position >= 100 ? 100 : _position,
                IsWon = _isWon,
                DiceValue = diceValue
            };

            var newPositionWithLadder = _gameFactory.ApplyLadder(_position);

            if (newPositionWithLadder != _position)
            {
                gameMatrix.Position = newPositionWithLadder;
                _positionHistory = new Dictionary<string, List<int>>();
                _positionHistory.GetValueOrDefault(userName)?.Add(newPositionWithLadder);
                gameMatrix.IsLadder = true;
                return gameMatrix;
            }

            var snakeBite = _gameFactory.SnakeBite(_position);
            if (snakeBite != _position)
            {
                gameMatrix.Position = snakeBite;
                _positionHistory = new Dictionary<string, List<int>>();
                _positionHistory.GetValueOrDefault(userName)?.Add(snakeBite);
                gameMatrix.IsSnakeBite = true;
            }

            var cardColor = _gameFactory.GetCardColor(_position);

            gameMatrix.CardColor = cardColor;

            return gameMatrix;
        }

        public void ResetGame()
        {
            _position = 0;
            _positionHistory = new Dictionary<string, List<int>>();
        }
        public GameMatrix RollDice(string userName)
        {
            _ = _positionHistory.TryAdd(userName, new List<int>());
            return Move(userName, _diceRoller.RollDice());
        }
    }
}
