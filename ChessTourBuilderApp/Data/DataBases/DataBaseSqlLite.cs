using ChessTourBuilderApp.Data.DataBases.Interfeses;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.DataBases
{
    internal class DataBaseSqlLite : IDataBase
    {
        static SqliteConnection sqliteConnection;
        static string sqlcon;
        static string paths;
        static SqliteCommand sqliteCommand;
        static SqlCommand sqlCommand;
        static SqliteDataReader reader;
        static SqlDataReader readerSQL;
        public static SqliteConnection temp;

        public bool ChangeConnection()
        {
            try
            {
                paths = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases.txt");
                string[] lines = File.ReadAllLines(paths);

                if (lines.Length != 4) return false;

                sqlcon = $"";
                sqliteConnection = new SqliteConnection(sqlcon);

                sqliteConnection.Open();
                SqliteCommand sqlCommand = new("select 1 from Organizer", sqliteConnection);
                bool result = sqlCommand.ExecuteNonQuery() > 0;
                sqliteConnection.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CloseCon()
        {
            sqliteCommand.Parameters.Clear();
            sqliteConnection.Close();
        }

        public SqlDataReader Conn(string str)
        {
            try
            {
                OpenConn(str);
                readerSQL = sqlCommand.ExecuteReader();
                return readerSQL;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ConnChange(string str)
        {
            try
            {
                OpenConn(str);
                bool result = sqliteCommand.ExecuteNonQuery() > 0;
                CloseCon();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ConnChange(string str, List<SqlParameter> list)
        {
            try
            {
                OpenConn(str, list);
                bool result = sqliteCommand.ExecuteNonQuery() > 0;
                CloseCon();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ConnChangeFull(string str, List<SqlParameter> list)
        {
            throw new NotImplementedException();
        }

        public bool ConnChangeFull(string str)
        {
            throw new NotImplementedException();
        }

        public bool ConnChangeTemp(string str)
        {
            throw new NotImplementedException();
        }

        public DataSet ConnDataSet(string str)
        {
            throw new NotImplementedException();
        }

        public DataSet ConnDataSetFull(string str)
        {
            throw new NotImplementedException();
        }

        public SqlDataReader ConnFull(string str)
        {
            throw new NotImplementedException();
        }

        public string GetTables()
        {
            string pathsSQL = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\БД.sql");
            return File.ReadAllText(pathsSQL);
        }

        public string NewConnection(string[] values)
        {
            throw new NotImplementedException();
        }

        public void OpenConn(string str)
        {
            sqliteConnection.Open();
            sqliteCommand = new SqliteCommand(str, sqliteConnection);
        }

        public void OpenConn(string str, List<SqlParameter> list)
        {
            sqliteConnection.Open();
            sqliteCommand = new SqliteCommand(str, sqliteConnection);
            foreach (var item in list)
            {
                sqliteCommand.Parameters.Add(item);
            }
        }

        public void OpenConn()
        {
            sqliteConnection.Open();
        }
    }
}
