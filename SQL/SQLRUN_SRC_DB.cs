using HaoYue.MHFUserSrv.Server.Common;
using Npgsql;
using System.Data;

namespace Axibug.MHFSaveAutoConverter.SQL
{
    internal static class SQLRUN_SRC_DB
    {
        static string connectionString => $"Host={Config.DictCfg["dbServer"]};Port={Config.DictCfg["dbPort"]};Database={Config.DictCfg["src_dbName"]};Username={Config.DictCfg["dbUser"]};Password={Config.DictCfg["dbPwd"]};";
        static Queue<NpgsqlConnection> QueueCon = new Queue<NpgsqlConnection>();
        static int PoolLimit = 2;

        static int GetPoolCount()
        {
            return QueueCon.Count;
        }

        static NpgsqlConnection GetSqlConnect()
        {
            lock (QueueCon)
            {
                NpgsqlConnection con;
                if (QueueCon.Count > 0)
                {
                    con = QueueCon.Dequeue();
                    if (con.State < ConnectionState.Open)
                    {
                        con.Dispose();
                        NpgsqlConnection newcon = new NpgsqlConnection(connectionString);
                        newcon.Open();
                        con = newcon;
                    }
                }
                else
                {
                    NpgsqlConnection newcon = new NpgsqlConnection(connectionString);
                    newcon.Open();
                    con = newcon;
                }
                return con;
            }
        }

        static void PushConnect(NpgsqlConnection con)
        {
            lock (QueueCon)
            {
                if (QueueCon.Count < PoolLimit && con.State >= ConnectionState.Open)
                {
                    QueueCon.Enqueue(con);
                }
                else
                {
                    if (con.State >= ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
        }

        public static bool QuerySQL(string strSql, out DataTable dt)
        {
            dt = null;
            if (string.IsNullOrEmpty(strSql))
                return false;

            NpgsqlConnection con = GetSqlConnect();
            bool bneedPush = true;
            try
            {
                NpgsqlDataAdapter sda = new NpgsqlDataAdapter(strSql, con);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                PushConnect(con);
                bneedPush = false;
                dt = ds.Tables[0];
                return true;
            }
            catch
            {
                if (bneedPush)
                {
                    PushConnect(con);
                }
                return false;
            }
        }

        public static bool ExcuteSQL(string strSql)
        {
            NpgsqlConnection con = GetSqlConnect();
            bool bneedPush = true;
            if (string.IsNullOrEmpty(strSql))
                return false;
            string strSqlinsert = strSql;
            int setnumbert = 0;
            try
            {
                using (NpgsqlCommand SqlCommand = new NpgsqlCommand(strSqlinsert, con))
                {
                    setnumbert = SqlCommand.ExecuteNonQuery();
                }
                PushConnect(con);
                bneedPush = false;
                return true;
            }
            catch (Exception ex)
            {
                if (bneedPush)
                {
                    PushConnect(con);
                }
                return false;
            }
        }

        public static DateTime ConvertTimestamptzToDatetime(object obj)
        {
            return TimeZoneInfo.ConvertTimeFromUtc((DateTime)obj, TimeZoneInfo.Local);
        }

    }
}
