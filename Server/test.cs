using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace Server
{
    public class test : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.Write(e.Data);
        }
    }
}
