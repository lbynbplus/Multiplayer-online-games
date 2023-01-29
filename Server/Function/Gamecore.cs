using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using Server;
using Server.Data;
using System.Collections;

namespace Server.Function
{
    public class Gamecore : WebSocketBehavior
    {
        public List<Player> playerlist= new List<Player>();
        public int[] AssPlayerID = new int[]{1,2,3,4,5,6};
        bool notfinish = false;

        public String SendID()
        {
            int i = Sessions.Count;
            Console.WriteLine(Sessions.Count.ToString());
            return AssPlayerID[--i].ToString();
        }

        protected override void OnOpen()
        {
            if(Sessions.Count >= 6)
            {
                Sessions.Broadcast("$%*&%$");
                Sessions.Broadcast(CreatData());
            }
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            switch (CheckNextStep())
            {
                case 0:
                    Console.WriteLine(e.Data);
                    if (e.Data == "hello")
                    {
                        if (Sessions.Count >= 6)
                        {
                            Send("The game has started, please wait for the next game to start.");
                        }
                        else
                        {
                            Send(SendID());
                            playerlist.Add(new Player() { });
                        }
                    }
                    else
                    {
                        Sessions.Broadcast(e.Data);
                        Sessions.Broadcast("Now "+Sessions.Count.ToString()+ " players waiting");
                    }
                    break;
                case 1:
                    MsgAnalysis(e.Data);
                    int tempid = HowSendMsg();
                    if (tempid!=0)
                    {
                        switch (CheckWhichState(tempid))
                        {
                            case 0:
                                Sessions.Broadcast("PPLMOVE$" + tempid.ToString() + "/" + playerlist[tempid].position);
                                break;
                            case 1:
                                Sessions.Broadcast("PPLTALK$" + tempid.ToString() + "/" + playerlist[tempid].msg);
                                break;
                        }
                        break;
                    }
                    else
                    { 
                        break;
                    }
                case 2:

                    break;
            }
        }

        public int CheckNextStep()
        {
            if(Sessions.Count >= 6 && notfinish) { return (int)Program.GameState.GameRoom; } 
            if(Sessions.Count >= 6 && notfinish == false) { return (int)Program.GameState.SettlementRoom;}
            else { return (int)Program.GameState.WaitingRoom;}
        }

        public string CreatData()
        {
            string t = null;
            for(int i = 0;i< AssPlayerID.Length;i++)
            {
                t = t + AssPlayerID[i].ToString();
                t = t + " ";
                t = t + playerlist[i].position.ToString();
                t = t + " ";
                t = t + playerlist[i].CardY.ToString();
                t = t + " ";
                t = t + playerlist[i].CardR.ToString();
                t = t + " ";
                t = t + playerlist[i].CardB.ToString();
                t = t + " ";
                t = t + playerlist[i].msg;
                t = t + ";";
            }
            return t;
        }
        
        public void MsgAnalysis(string msg)
        {

            string[] words = msg.Split(';');
            for(int i=0; i< words.Length;i++)
            {
                string[] strings= words[i].Split(' ');
                for(int n=0;n< strings.Length;n++)
                {
                    switch(n)
                    {
                        case 0:
                            playerlist[i].id = Convert.ToInt32(strings[n]); break;
                        case 1:
                            playerlist[i].position = Convert.ToInt32(strings[n]); break;
                        case 2:
                            playerlist[i].CardY = Convert.ToInt32(strings[n]); break;
                        case 3:
                            playerlist[i].CardR = Convert.ToInt32(strings[n]); break;
                        case 4:
                            playerlist[i].CardB = Convert.ToInt32(strings[n]); break;
                        case 5:
                            playerlist[i].msg = Convert.ToString(strings[n]);break;
                    }
                }
            }
        }

        public int HowSendMsg()
        {
            for(int i=0; i<playerlist.Count;i++)
            {
                if (!(playerlist[i].msg == playerlist[i].MsgCheck) || !(playerlist[i].position ==playerlist[i].poscheck))
                {
                    return playerlist[i].id;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        public int CheckWhichState(int id)
        {
            if (playerlist[id].position != playerlist[id].poscheck)
            {
                playerlist[id].poscheck = playerlist[id].position;
                return 0;
            }
            else
            {
                playerlist[id].MsgCheck= playerlist[id].msg;
                return 1;
            }
        }

    }
}
