using System;
using System.Collections.Generic;
using System.Text;

namespace _159333project.Server.Server.data
{
    public class MessageCarrior
    {
        public class ServerMessageCarrior
        {
            public string sendto;//只发给某个玩家
            public string except;//不包含某个玩家

            public GameMessage message;
        }

        public class ClientMessageCarrior
        {
            public string id; //发送者ID

            public GameMessage message;
        }
    }
}
