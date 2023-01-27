using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data
{
    internal class PlayerPiece
    {
        public int[] numberofPieces = new int[6];
        public int piecePosition { get; set; }
        public string pieceColor { get; set; }
    }
}
