using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IGameService
    {
        public GameMatrix RollDice();
        public void ResetGame();

    }

    public class GameMatrix
    {
        public int DiceValue { get; set; }

        public int Position { get; set; }

        public bool IsWon { get; set; }

        public bool IsSnakeBite { get; set; }

        public bool IsLadder { get; set; }

    }
}
