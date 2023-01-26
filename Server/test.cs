using WebSocketSharp.Server;
using WebSocketSharp;

namespace Server
{
    public class test : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Sessions.Broadcast(Sessions.Count.ToString());
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.ActiveIDs.GetEnumerator();
            Sessions.IDs.GetEnumerator().MoveNext();
            Console.Write(e.Data);
            Console.Write(Sessions.Count.ToString());
            //Sessions.SendTo(e.Data, Sessions.ActiveIDs.GetEnumerator().Current);
            if (Sessions.IDs.GetEnumerator().MoveNext())
            {
                Console.WriteLine(Sessions.IDs.First().ToString());
                Console.Write("sss");
            }
            Sessions.SendTo(e.Data, Sessions.IDs.First().ToString());
            Sessions.Broadcast(Sessions.IDs.First().ToString());
        }
    }
}
