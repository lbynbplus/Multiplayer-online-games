using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using _159333project.Server.Server.data;
using System.Collections.Generic;
using _159333project.Client;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace _159333project.Server.Server
{
    public class Server
    {
        public GameMessage msg;
        public string myIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
        public static List<TcpClient> clients = new List<TcpClient>();
        private static TcpListener listener;

        private static void DoAcceptTcpclient(IAsyncResult State)
        {
            /*                                */
            /* Handling multiple client access*/
            /*                                */
            TcpListener listener = (TcpListener)State.AsyncState;

            TcpClient client = listener.EndAcceptTcpClient(State);

            clients.Add(client);

            Console.WriteLine("\nReceive new client:{0}", client.Client.RemoteEndPoint.ToString());
            //Open threads to continuously receive data from the client
            Thread myThread = new Thread(new ParameterizedThreadStart(printReceiveMsg));
            myThread.Start(client);

            listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpclient), listener);
        }

        private static void printReceiveMsg(object reciveClient)
        {

        }
        public static void Main()
        {
            string myIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            TcpListener server = null;
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse(myIP);

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();
            Console.WriteLine("Start listening successfully");
            listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpclient), listener);
            Console.ReadKey();
        }
    }
}
