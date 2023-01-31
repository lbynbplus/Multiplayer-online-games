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
using System.Data.SQLite;

namespace Server.Function
{
    public class Gamecore : WebSocketBehavior
    {
        public List<Player> playerlist= new List<Player>();
        bool notfinish = false;
        public String PPLCheck = "N";

        public int idl()
        {
            int i = 0;
            SQLiteDataReader sqlite_datareader;
            SQLiteConnection conn = new SQLiteConnection(Program.dbfile);
            conn.Open();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM PlayerTable";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while(sqlite_datareader.Read())
            {
                i++;
            }
            conn.Close();
            return i;
        }
        protected override void OnOpen()
        {
            Player player= new Player() { ID = idl(), Name = "NULL", Id = ID, CardB = 0, CardG = 0, CardY = 0, position = 0};
            Program.AddPlayer(player);
            int iii = idl();
            Console.WriteLine(iii.ToString());
        }

       

        protected override void OnMessage(MessageEventArgs e)
        {
            switch(CheckInfo(e.Data))
            {
                case -1:
                    break;
                case 0:
                    if(PPLCheck == "FULL")
                    {
                        Send("FULL$");
                        break;
                    }
                    playerlist[CheckHow(ID)].Name = InfoContect(e.Data);
                    break;
                case 1:
                    Sessions.Broadcast("TALK$" + playerlist[CheckHow(ID)].Name + ": " + InfoContect(e.Data));
                    Console.WriteLine(CheckHow(ID).ToString() + " " + playerlist[CheckHow(ID)].Name + ": " + InfoContect(e.Data));
                    break;
                case 2:
                    PlayerMove(playerlist[CheckHow(ID)], Convert.ToInt32(InfoContect(e.Data)));
                    Sessions.Broadcast("STATE$" + CreatData());
                    break;

            }
        }

        public int CheckHow(String ID)
        {
            return 0;
        }
        public int CheckInfo(String msg)
        {
            string[] words = msg.Split('$');
            if (words[0] == "NAME")
            {
                return 0;
            }
            if (words[0] == "TALK")
            {
                return 1;
            }
            if((words[0] == "MOVE"))
            {
                return 2;
            }
                return -1;
        }

        public String InfoContect(String msg)
        {
            string? info = null;
            string[] words = msg.Split('$');
            for(int i=1;i<words.Length; i++)
            {
                info += words[i]; 
            }
            return info;
        }
        
        public void PlayerMove(Player player,int move)
        {
            player.position += move;
        }
        
        public int CheckNextStep()
        {
            if(Sessions.Count >= 6 && notfinish) { return (int)Program.GameState.GameRoom; } 
            if(Sessions.Count >= 6 && notfinish == false) { return (int)Program.GameState.SettlementRoom;}
            else { return (int)Program.GameState.WaitingRoom;}
        }

        
        
         public string CreatData()
        {
            string? t = null;
            for(int i = 0;i< playerlist.Count;i++)
            {
                t = t + playerlist[i].Name+ " ";
                t = t + playerlist[i].position.ToString();
                t = t + " ";
                t = t + playerlist[i].CardY.ToString();
                t = t + " ";
                t = t + playerlist[i].CardG.ToString();
                t = t + " ";
                t = t + playerlist[i].CardB.ToString();
                t = t + " ";
            }
            return t;
        }
        
        /*
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
        */


    }
}
