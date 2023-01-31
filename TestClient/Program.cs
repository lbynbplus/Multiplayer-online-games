using System;
using WebSocketSharp;

namespace TestClient
{
    class Program
    {
        static string temp = "Anonymous";
        static void Main(String[] args)
        {
            WebSocket ws = new WebSocket("ws://127.0.0.1:8205/Gamecore");
            ws.OnMessage += Ws_OnMessage;

            ws.Connect();
            Console.WriteLine("Enter your name");
            temp = Console.ReadLine();
            ws.Send("NAME$" + temp);
            while (true)
            { 
               ws.Send("TALK$" + Sender());
            }
                
        }

        private static void Ws_OnMessage(object? sender, MessageEventArgs e)
        {
            switch(CheckState(e.Data))
            {
                case -1:
                    break;
                case 0:
                    if(CheckMine(e.Data) == 1)
                    {
                        break;
                    }
                    Console.WriteLine(info(e.Data));
                    break;
                case 1:

                    break; 
                case 2:
                    Console.WriteLine("The server is full of connected players and you can only go into watch mode.");
                    break;
            }
        }

        public static int CheckState(String msg)
        {
            string[] words = msg.Split('$');
            if (words[0] == "TALK")
            {
                return 0;
            }
            if (words[0] == "STATE")
            {
                return 1;
            }
            if(words[0] == "FULL")
            {
                return 2;
            }
            return -1;

        }

        private static int CheckMine(String msg)
        {
            string[] words = msg.Split('$');
            string[] word = words[1].Split(':');
            if (word[0] == temp)
            {
                return 1;
            }
            return 0;
        }

        public static string info(string msg)
        {
            string? info = null;
            string[] words = msg.Split('$');
            for (int i = 1; i < words.Length; i++)
            {
                info += words[i];
            }
            return info;
        }

        public static String Sender()
        {
            string? temp = Console.ReadLine();
            return temp;
        }
    }
}