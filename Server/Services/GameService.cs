using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class GameService : IGameService
    {
        IDiceRoller _diceRoller;
        IGameFactory _gameFactory;
        private bool _isWon = false;
        private int _position = 0;
        private static List<int> _positionHistory = new List<int>();

        public GameService(IDiceRoller diceRoller,IGameFactory gameFactory)
        {
            _diceRoller = diceRoller;
            _gameFactory = gameFactory;
        }

        public GameMatrix Move(int diceValue)
        {
            if (_positionHistory.Sum() <= 100) 
            {
                _positionHistory.Add(diceValue);
                _position = _positionHistory.Sum();
            }

            _isWon = (_position >= 100);

            var gameMatrix = new GameMatrix()
            {
                Position = _position >= 100 ? 100 : _position,
                IsWon = _isWon,
                DiceValue = diceValue
            };

            var newPositionWithLadder=_gameFactory.ApplyLadder(_position);
            
            if (newPositionWithLadder != _position)
            {
                gameMatrix.Position = newPositionWithLadder;
                _positionHistory = new List<int>();
                _positionHistory.Add(newPositionWithLadder);
                gameMatrix.IsLadder = true;
                return gameMatrix;
            }

            var snakeBite = _gameFactory.SnakeBite(_position);
            if(snakeBite != _position)
            {
                gameMatrix.Position = snakeBite;
                _positionHistory = new List<int>();
                _positionHistory.Add(snakeBite);
                gameMatrix.IsSnakeBite = true;
            }

            return gameMatrix;
        }

        public void ResetGame()
        {
            _position = 0;
            _positionHistory = new List<int>();
        }
        public GameMatrix RollDice()
        {
            return Move(_diceRoller.RollDice());
        }
    }
}
