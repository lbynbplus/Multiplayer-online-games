using System;
using WebSocketSharp;

namespace TestClient
{
    class Program
    {
        static bool CheckID = true;
        public static String PlayerID = "TEMP";
        private static String Temp = "TEMP";

        static void Main(String[] args)
        {
            WebSocket ws = new WebSocket("ws://127.0.0.1:8205/WaitingRoom");
            ws.OnMessage += Ws_OnMessage;

                ws.Connect();
                ws.Send("hello");
                while (true)
                {
                    ws.Send(Sender());
                }
                
        }

        private static void Ws_OnMessage(object? sender, MessageEventArgs e)
        {
            if(CheckID)
            {
                PlayerID = e.Data;
                CheckID = false;
            }
            if(Temp == e.Data)
            {
                Temp = e.Data;
            }
            else
            {
                Console.WriteLine(e.Data);
            }
            //throw new NotImplementedException();
        }

        public static String Sender()
        {
            string temp = Console.ReadLine();
            Temp = PlayerID + ":" + temp;
            return PlayerID+":"+temp;
        }
    }
}