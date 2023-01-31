using System;
using System.Net;
using System.Text;
using WebSocketSharp.Server;
using WebSocketSharp;
using Server.Function;
using Server.Data;
using System.Collections;
using System.Data.SQLite;
using System.Data.Entity;
using System.Data;

namespace Server
{
    class Program
    {
        public static string dbfile = "Data Source= database.db; Version = 3; New = True; Compress = True; ";
        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source= database.db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sqlite_conn;
        }
        static void CreateTable(SQLiteConnection conn)
        {
            SQLiteCommand sqliteCmd = conn.CreateCommand();
            sqliteCmd.CommandText = "CREATE TABLE IF NOT EXISTS PlayerTable (ID INT Primary Key, Name text, SessionsID text, CardY INT, CardG INT, CardB INT, position INT)";
            sqliteCmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void AddPlayer(Player player)
        {
            SQLiteConnection conn = new SQLiteConnection(dbfile);
            conn.Open();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"INSERT INTO PlayerTable(ID, Name, SessionsID, CardY, CardG, CardB, position) VALUES(" +
                $"{player.ID}, '{player.Name}', '{player.Id}', {player.CardY}, {player.CardG}, {player.CardB}, {player.position})";
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static int PLAYERNUM;
        public enum GameState
        {
            WaitingRoom,
            GameRoom,
            SettlementRoom

        }
        static void Putplayer()
        {
            SQLiteConnection conn = CreateConnection();
            CreateTable(conn);
            Console.WriteLine("Please enter the number of players the game server can accept this time (the number is limited to 0 to 10 people)");
            while (true) 
            {
                string? temp = Console.ReadLine();
                int i = Convert.ToInt32(temp);
                if (0 < i)
                {
                    if (i < 10)
                    {
                        PLAYERNUM = i;
                        Console.WriteLine("Great, the total number of players for this game will be " + PLAYERNUM);
                        break;
                    }
                }
                Console.WriteLine("You have entered an invalid number of people.");
                Console.WriteLine("Please enter the number of people from a new entry.");
            }
        }
        static void Main(string[] args)
        {
            Putplayer();
            WebSocketServer Server = new WebSocketServer("ws://127.0.0.1:8205");
            Server.AddWebSocketService<Gamecore>("/Gamecore");
            Server.Start();
            Console.WriteLine("Server Start on :ws://127.0.0.1:8205");

            Console.ReadKey();
            Server.Stop();

        }
    }
}