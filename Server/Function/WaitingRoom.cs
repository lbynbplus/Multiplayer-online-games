using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace Server.Function
{
    public class WaitingRoom : WebSocketBehavior
    {
        private List<String> AssPlayerID = new List<String>{"1", "2", "3", "4", "5", "6"};


        protected override void OnOpen()
        {
          while (true)
            {
                Sessions.Broadcast(Sessions.Count.ToString());
                if (Sessions.Count >= 6) break;
            }
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast(e.Data);
        }
    }
}
