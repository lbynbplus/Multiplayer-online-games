using WebSocketSharp.Server;
using WebSocketSharp;
using Server.Data;
using System.Data.SQLite;

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
            sqliteCmd.CommandText = "CREATE TABLE IF NOT EXISTS SampleTable (Col1 VARCHAR(20), Col2 VARCHAR(20), Col3 INT)";
            sqliteCmd.ExecuteNonQuery();

        }

        static void InsertID(SQLiteConnection conn, String id)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable(Col1, Col2, Col3) VALUES('" + id + " ', G:0,Y:0,B:0, 0); ";
            sqlite_cmd.ExecuteNonQuery();
        }


        static string getID(SQLiteConnection conn, int who)
        {
            int i = 0;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                i++;
                if (i == who)
                {
                    string myreader = sqlite_datareader.GetString(1);
                    conn.Close();
                    return myreader;
                }
            }
            conn.Close();
            return "NULL";
        }
    }
}
