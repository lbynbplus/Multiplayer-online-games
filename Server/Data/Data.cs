using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data
{
//     public class coding
//     {
      
//     }
//     public class Data
//     {
        
//     }

   public class Game
   {

        //Method to add players
        public void addPlayer()
        {
            var playerLists = new ArrayList();
            Player player1 = new Player();
            Player player2 = new Player();
            Player player3 = new Player();
            Player player4 = new Player();
            Player player5 = new Player();
            Player player6 = new Player();

            playerLists.Add(player1);
            playerLists.Add(player2);
            playerLists.Add(player3);
            playerLists.Add(player4);
            playerLists.Add(player5);
            playerLists.Add(player6);
        }


    }

}
