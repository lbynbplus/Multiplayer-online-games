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
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Xml.Linq;

namespace Server.Function
{
    public class Gamecore : WebSocketBehavior
    {
        public List<Player> playerlist= new List<Player>();
        bool gamestart = false;
        int PlayerCount = 0;
        int playernow = 0;
        bool AlreadyStart = false;

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
            PlayerCount = idl();
            if (PlayerCount != --Program.PLAYERNUM)
            {
                playerlist.Add(new Player() { ID = idl(), Name = "NULL", Id = ID, CardB = 0, CardG = 0, CardY = 0, position = 0 });
                Player player = new Player() { ID = idl(), Name = "NULL", Id = ID, CardB = 0, CardG = 0, CardY = 0, position = 0 };
                Program.AddPlayer(player);
                Console.WriteLine(PlayerCount.ToString());
                if(idl() == --Program.PLAYERNUM)
                {
                    gamestart = true;
                }
            }
            else
            {
                gamestart = true;
                return;
            }
        }

        int CheckExist()
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteConnection conn = new SQLiteConnection(Program.dbfile);
            conn.Open();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM PlayerTable";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if(ID == sqlite_datareader.GetString(2))
                {
                    return sqlite_datareader.GetInt32(0);
                }
            }
            conn.Close();
            return -1;
        }

        
       bool Change(String col,int num)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(Program.dbfile);
                conn.Open();
                string cmdcode = "UPDATE PlayerTable" + " SET "+ col +" = " + num + " where ID='" + PlayerCount + "'";
                SQLiteCommand command = new SQLiteCommand(cmdcode, conn);
                command.ExecuteNonQuery();
                conn.Close();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        bool ChangeName(String Name)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(Program.dbfile);
                conn.Open();
                string cmdcode = "UPDATE PlayerTable" + " SET Name = " + Name + " where ID='" + PlayerCount + "'";
                SQLiteCommand command = new SQLiteCommand(cmdcode, conn);
                command.ExecuteNonQuery();
                conn.Close();
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public void Start()
        {
            Console.WriteLine("Game Start");
            Sessions.Broadcast("GAMESTART$");
            SQLiteConnection conn = new SQLiteConnection(Program.dbfile);
            conn.Open();
            Random rd = new Random();
            for (int i=0;i<PlayerCount;i++)
            {
                string cmdcode = "UPDATE PlayerTable" + " SET position = " + rd.Next(1,62) + " where ID='" + i + "'";
                SQLiteCommand command = new SQLiteCommand(cmdcode, conn);
                command.ExecuteNonQuery();
            }
            conn.Close();
            Sessions.Broadcast(CreatData());
            Sessions.SendTo("YOUROUND$0", findppl(0));
            AlreadyStart= true;
        }

        public string findppl(int now)
        {
            int temp = 0;
            SQLiteDataReader sqlite_datareader;
            SQLiteConnection conn = new SQLiteConnection(Program.dbfile);
            conn.Open();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT SessionsID FROM PlayerTable";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (temp == now)
                {
                    return sqlite_datareader.GetString(2);
                }
                temp++;
            }
            conn.Close();
            return null;
        }
        public void playerloop(int now)
        {
            now++;
            if (now == 7)
            {
                now = 0;
            }
            Sessions.SendTo("YOUROUND$", findppl(now));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            switch(CheckInfo(e.Data))
            {
                case -1:
                    break;
                case 0:
                    if(CheckExist() != -1)
                    {
                        playerlist[0].Name = InfoContect(e.Data);
                        Console.WriteLine("exist" + CheckExist().ToString());
                        ChangeName(InfoContect(e.Data));
                    }
                    else
                    {
                        Send("FULL$");
                        if(AlreadyStart == false)
                        {
                            Start();
                        }
                    }
                    break;
                case 1:
                    Sessions.Broadcast("TALK$" + playerlist[0].Name + ": " + InfoContect(e.Data));
                    Console.WriteLine(playerlist[0].Name + ": " + InfoContect(e.Data));
                    if(idl() == Program.PLAYERNUM)
                    {
                        if(AlreadyStart == false)
                        {
                            Start();
                        }
                    }
                    break;
                case 2:
                    Change("position", Convert.ToInt32(InfoContect(e.Data)));
                    Sessions.Broadcast("STATE$" + CreatData());
                    break;
                case 3:
                    if (idl() == Program.PLAYERNUM)
                    {
                        if (AlreadyStart == false)
                        {
                            Start();
                        }
                    }
                    break;
                case 4:
                    AlreadyStart = false;
                    break;

            }
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
            if(words[0] == "MOVE")
            {
                return 2;
            }
            if(words[0] == "REFRESH")
            {
                return 3;
            }
            if (words[0] == "STARTG")
            {
                return 4;
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

         public string CreatData()
        {
            string? t = null;
            SQLiteDataReader sqlite_datareader;
            SQLiteConnection conn = new SQLiteConnection(Program.dbfile);
            conn.Open();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM PlayerTable";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while(sqlite_datareader.Read()) 
            {
                t = t + sqlite_datareader.GetString(1) + " ";
                t = t + sqlite_datareader.GetString(7).ToString();
                t = t + " ";
                t = t + sqlite_datareader.GetInt32(4).ToString();
                t = t + " ";
                t = t + sqlite_datareader.GetInt32(4).ToString();
                t = t + " ";
                t = t + sqlite_datareader.GetInt32(4).ToString();
                t = t + ":";
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
