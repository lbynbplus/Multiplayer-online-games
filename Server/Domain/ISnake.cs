using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain
{
    public interface ISnake : IPosition
    {
        public int GetPosition(int position);

    }

    public class Snake : Position, ISnake
    {
        private List<Position> _snakeBites = new List<Position>()
        {
            new Position()
            {
                Start = 50,
                End =10
            },
            new Position()
            {
                Start = 25,
                End =1
            },
            new Position()
            {
                Start = 37,
                End =5
            },
            new Position()
            {
                Start = 55,
                End =11
            },
            new Position()
            {
                Start = 98,
                End = 20
            }
    };

        /// <summary>
        /// 根据蛇获取新坐标
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetPosition(int position)
        {
            var snakeBite = _snakeBites.SingleOrDefault(x => x.Start == position);
            if (snakeBite != null)
                return snakeBite.End;
            else
                return position;
        }
    }

}
