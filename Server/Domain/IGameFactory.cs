using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain
{
    public interface IGameFactory
    {
        public int ApplyLadder(int postion);

        public int SnakeBite(int postion);

        public CardColor GetCardColor(int postion);
    }
}
