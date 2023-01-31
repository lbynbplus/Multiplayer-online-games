using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data
{
    public class Player
    {
        public String? Id { get; set; }
        public String? Name { get; set; }
        public int ID { get; set; }
        public int position { get; set; }
        public int CardY { get; set; }
        public int CardG { get; set; }
        public int CardB { get; set; }
    }
}
