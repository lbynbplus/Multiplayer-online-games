using System;
using System.Net;
using System.Text;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace Server
{
  class Program
    {
       static void Main(string[] args)
        {
            WebSocketServer Server = new WebSocketServer("ws://127.0.0.1:8205");
            Server.Start();
            Console.WriteLine("Server Start");

            Console.ReadKey();
            Server.Stop();

        }
    }
}