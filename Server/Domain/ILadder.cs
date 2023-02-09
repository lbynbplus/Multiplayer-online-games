using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain
{
    public interface ILadder 
    {
        public int GetPosition(int position);
    }

    public class Ladder : Position, ILadder
    {
        private List<Position> _ladderPosition = new List<Position>()
        {
            new Position()
            {
                Start = 3,
                End =23
            },
            new Position()
            {
                Start = 48,
                End =68
            },
            new Position()
            {
                Start = 31,
                End =71
            },
            new Position()
            {
                Start = 60,
                End =90
            }
        };

        public int GetPosition(int position)
        {
            var ladder = _ladderPosition.SingleOrDefault(x => x.Start == position);

            if (ladder != null)
                return ladder.End;
            else
                return position;
        }
    }
}
