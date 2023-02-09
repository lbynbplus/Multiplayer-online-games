using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IGameService
    {
        public GameMatrix RollDice(string userName);
        public void ResetGame();

    }

    public class GameMatrix
    {
        public int DiceValue { get; set; }

        public int Position { get; set; }

        public bool IsWon { get; set; }

        public bool IsSnakeBite { get; set; }

        public bool IsLadder { get; set; }

        /// <summary>
        /// 卡片颜色
        /// </summary>
        public CardColor CardColor { get; set; }
    }
}
