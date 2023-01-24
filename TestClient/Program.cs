﻿using System;
using WebSocketSharp;

namespace TestClient
{
    class Program
    {
        static void Main(String[] args)
        {
            using (WebSocket ws = new WebSocket("ws://127.0.0.1:8205/test"))
            {
                ws.Connect();
                ws.Send("Connected");

                Console.ReadKey();
                ws.Close();
            }                             
        }
    }
}