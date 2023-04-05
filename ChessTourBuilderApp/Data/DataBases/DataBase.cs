using ChessTourBuilderApp.Data.DataBases.Interfeses;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.DataBases
{
    internal class DataBase : IDataBase
    {
        static string sqlcon;
        static string paths;
        static SqlConnection sqlConnection;
        static SqlCommand sqlCommand;
        static SqlDataReader reader;
        public static SqlConnection temp;

        public string GetTables() 
        {
            string pathsSQL = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\БД.sql");
            return File.ReadAllText(pathsSQL);
        }

        public SqlDataReader Conn(string str)
        {
            try
            {
                OpenConn(str);
                reader = sqlCommand.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataSet ConnDataSet(string str)
        {
            try
            {
                sqlConnection.Open();
                SqlDataAdapter adapter = new(str, sqlConnection);
                DataSet ds = new();
                adapter.Fill(ds);
                CloseCon();
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ConnChangeTemp(string str)
        {
            try
            {
                if (temp.State == ConnectionState.Closed) temp.Open();
                sqlCommand = new SqlCommand(str, temp);
                sqlCommand.ExecuteNonQuery();
                temp.Close();
                return true;
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
                bool result = sqlCommand.ExecuteNonQuery() > 0;
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
                bool result = sqlCommand.ExecuteNonQuery() > 0;
                CloseCon();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void OpenConn(string str)
        {
            sqlConnection.Open();
            sqlCommand = new SqlCommand(str, sqlConnection);
        }

        public void OpenConn(string str, List<SqlParameter> list)
        {
            sqlConnection.Open();
            sqlCommand = new SqlCommand(str, sqlConnection);
            foreach (var item in list)
            {
                sqlCommand.Parameters.Add(item);
            }
        }

        public void OpenConn()
        {
            sqlConnection.Open();
        }

        public void CloseCon()
        {
            sqlCommand.Parameters.Clear();
            sqlConnection.Close();
        }

        public bool ChangeConnection()
        {
            try
            {
                paths = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases.txt");
                string[] lines = File.ReadAllLines(paths);

                if (lines.Length != 4) return false;

                sqlcon = $"Data Source = {lines[0]}; " +
                                      $"Initial Catalog = {lines[1]}; " +
                                      $"User ID = {lines[2]};" +
                                      $"Password = {lines[3]};" +
                                      $"Trusted_Connection = true;" +
                                      $"TrustServerCertificate = true;" +
                                      $"Encrypt = false;" +
                                      $"Integrated Security = true;";
                sqlConnection = new SqlConnection(sqlcon);

                sqlConnection.Open();
                SqlCommand sqlCommand = new("select 1 from Organizer", sqlConnection);
                bool result = sqlCommand.ExecuteNonQuery() > 0;
                sqlConnection.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string NewConnection(string[] values)
        {
            SqlCommand sqlCommand;

            try
            {
                temp = new SqlConnection($"Data Source = {values[0]}; " +
                             $"Initial Catalog = {values[1]}; " +
                             $"User ID = {values[2]};" +
                             $"Password = {values[3]};" +
                             $"Trusted_Connection = true;" +
                             $"TrustServerCertificate = true;" +
                             $"Encrypt = false;" +
                             $"Integrated Security = true;");
                temp.Open();
                sqlCommand = new SqlCommand($"use {values[1]}", temp);
                sqlCommand.ExecuteNonQuery();
                temp.Close();
            }
            catch
            {
                return "NoDB";
            }

            try
            {
                temp.Open();
                sqlCommand = new SqlCommand($"select 1 from Organizer", temp);
                sqlCommand.ExecuteNonQuery();
                temp.Close();
            }
            catch
            {
                return "NoTable";
            }

            sqlConnection = temp;

            using (StreamWriter w = new(paths))
            {
                w.WriteLine(values[0]);
                w.WriteLine(values[1]);
                w.WriteLine(values[2]);
                w.WriteLine(values[3]);
            }

            return "ok";
        }

        public bool ConnChangeFull(string str, List<SqlParameter> list)
        {
            try
            {
                SqlCommand sqlCommand = new(str, sqlConnection);
                foreach (var item in list) sqlCommand.Parameters.Add(item);
                bool result = sqlCommand.ExecuteNonQuery() > 0;
                sqlCommand.Parameters.Clear();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SqlDataReader ConnFull(string str)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(str, sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataSet ConnDataSetFull(string str)
        {
            try
            {
                SqlDataAdapter adapter = new(str, sqlConnection);
                DataSet ds = new();
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ConnChangeFull(string str)
        {
            try
            {
                SqlCommand sqlCommand = new(str, sqlConnection);
                bool result = sqlCommand.ExecuteNonQuery() > 0;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
