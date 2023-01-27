using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data
{
    public class coding
    {
      
    }
    public class Data
    {
        
    }

    //public class Board
    //{
    //    Player players;
    //    Card cards;
    //    PlayerPiece playerPieces;
    //}

    //players
    public class Player
    {
        public int position { get; set; }
        public string playername { get; set; }
        public int playerid { get; set; } = 1;

    }
    //Cards
    public class Card
    {
        public int cardName { get; set; }
        public int cardColor { get; set; }
     
    }

    //Player Piece
    public class PlayerPiece
    {
        public int[] numberofPieces = new int[6];
        public int piecePosition { get; set;}
        public string pieceColor { get; set; }
    }

}
