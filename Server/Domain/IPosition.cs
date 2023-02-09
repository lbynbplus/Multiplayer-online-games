using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain
{
    public interface IPosition
    {
        public int Start { get; set; }

        public int End { get; set; }

    }

    public class Position : IPosition
    {
        public int Start { get; set; }
        public int End { get; set; }

    }
}
