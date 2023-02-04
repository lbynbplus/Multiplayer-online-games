using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain
{
    public class GameFactory : IGameFactory
    {
        ILadder _ladder;
        ISnake _snake;
        public GameFactory(ILadder ladder,ISnake snake)
        {
            _ladder = ladder;
            _snake = snake;
        }

        public int ApplyLadder(int postion)
        {
            return (_ladder.GetPosition(postion));
        }

        public int SnakeBite(int postion)
        {
            return (_snake.GetPosition(postion));
        }
    }
}
