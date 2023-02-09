using System;

namespace Server.Domain
{
    public interface IDiceRoller
    {
        /// <summary>
        /// 投骰子
        /// </summary>
        /// <returns></returns>
        public int RollDice();
    }
}
