using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using Server;

namespace Server.Function
{
    public class WaitingRoom : WebSocketBehavior
    {
        public int[] AssPlayerID = new int[]{1,2,3,4,5,6};

        public String SendID()
        {
            int i = Sessions.Count;
            Console.WriteLine(Sessions.Count.ToString());
            return AssPlayerID[--i].ToString();
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine(e.Data);
            if(e.Data == "hello") { Send(SendID()); } else
            {
                Sessions.Broadcast(e.Data);
            }
        }

        public String CheckNextStep()
        {
            if(Sessions.Count >= 6) { return Program.GameState.GameRoom.ToString(); } else
            {
                return Program.GameState.WaitingRoom.ToString();
            }
        }
    }
}
