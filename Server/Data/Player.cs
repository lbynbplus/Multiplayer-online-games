using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data
{
    public class Player
    {
        public int position { get; set; }
        public string playername { get; set; }
        public int playerid { get; set; } = 1;
    }
}
