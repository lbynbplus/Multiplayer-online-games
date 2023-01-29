using System;
using System.Net;
using System.Text;
using WebSocketSharp.Server;
using WebSocketSharp;
using Server.Function;
using Server.Data;
using System.Collections;

namespace Server
{
  class Program
    {      
       public enum GameState
        {
            WaitingRoom,
            GameRoom,
            SettlementRoom

        }
       static void Main(string[] args)
        {
            WebSocketServer Server = new WebSocketServer("ws://127.0.0.1:8205");
            Server.AddWebSocketService<test>("/test");
            Server.AddWebSocketService<Gamecore>("/Gamecore");
            Server.Start();
            Console.WriteLine("Server Start on :ws://127.0.0.1:8205");

            Console.ReadKey();
            Server.Stop();

        }
    }
}