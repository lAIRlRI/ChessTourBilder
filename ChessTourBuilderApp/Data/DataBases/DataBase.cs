﻿using ChessTourBuilderApp.Data.HelpClasses;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.DataBases
{
    internal class DataBase
    {
        static string paths;
        static string pathsSQL;
        static string flag;
        public static IDbConnection connection;
        public static SqliteConnection tempLite;
        public static bool serverOrLite = true;
 
        public static List<IDbDataParameter> SetParameters(List<ParametrBD> parametrs)
        {
            List<IDbDataParameter> temp = new();

            if (serverOrLite)
            {
                foreach (var item in parametrs)
                {
                    SqlParameter sqlParameter = new()
                    {
                        ParameterName = item.ParameterName,
                        Value = item.ParameterValue
                    };
                    temp.Add(sqlParameter);
                }
            }
            else 
            {
                foreach (var item in parametrs)
                {
                    SqliteParameter sqlParameter = new()
                    {
                        ParameterName = item.ParameterName,
                        Value = item.ParameterValue
                    };
                    temp.Add(sqlParameter);
                }
            }
            return temp;
        }

        public static string GetFlag()
        {
            flag = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\FlagBD.txt");
            return File.ReadAllText(flag);
        }

        public static string GetTables()
        {
            pathsSQL = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\БД.sql");
            return File.ReadAllText(pathsSQL);
        }

        public static string GetTablesLite()
        {
            pathsSQL = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\БДLite.txt");
            return File.ReadAllText(pathsSQL);
        }

        public static void OpenConn()
        {
            connection.Open();
        }

        public static void CloseCon()
        {
            connection.Close();
        }

        public static bool ChangeConnection()
        {
            try
            {
                paths = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\serverSetting.txt");
                string[] lines = File.ReadAllLines(paths);

                if (lines.Length != 4) return false;

                string sqlcon = $"Data Source = {lines[0]}; " +
                                      $"Initial Catalog = {lines[1]}; " +
                                      $"User ID = {lines[2]};" +
                                      $"Password = {lines[3]};" +
                                      $"Trusted_Connection = true;" +
                                      $"TrustServerCertificate = true;" +
                                      $"Encrypt = false;" +
                                      $"Integrated Security = true;";
                connection = new SqlConnection(sqlcon);

                connection.Open();
                using var sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "select 1 from Organizer";
                sqlCommand.ExecuteNonQuery();
                connection.Close();

                using (StreamWriter w = new(flag))
                {
                    w.WriteLine("1");
                }
                serverOrLite = true;
                StaticResouses.dBQ = new ServerQ();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ChangeConnectionLite()
        {
            try
            {
                string pathsLite = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\local.db");
                string lines = File.ReadAllText(pathsLite);

                if (lines == null) return false;

                connection = new SqliteConnection("DataSource = " + pathsLite + ";Mode = ReadWrite;");

                connection.Open();
                using var sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "select 1 from Organizer";
                sqlCommand.ExecuteNonQuery();
                connection.Close();

                using (StreamWriter w = new(flag))
                {
                    w.WriteLine("0");
                }
                serverOrLite = false;
                StaticResouses.dBQ = new LiteQ();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string NewConnection(string[] values)
        {
            try
            {
                connection = new SqlConnection($"Data Source = {values[0]}; " +
                             $"Initial Catalog = {values[1]}; " +
                             $"User ID = {values[2]};" +
                             $"Password = {values[3]};" +
                             $"Trusted_Connection = true;" +
                             $"TrustServerCertificate = true;" +
                             $"Encrypt = false;" +
                             $"Integrated Security = true;");
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = $"use {values[1]}";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                return "NoDB";
            }

            try
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "select 1 from Organizer";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                return "NoTable";
            }


            paths = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\serverSetting.txt");

            using (StreamWriter w = new(paths))
            {
                w.WriteLine(values[0]);
                w.WriteLine(values[1]);
                w.WriteLine(values[2]);
                w.WriteLine(values[3]);
            }

            using (StreamWriter w = new(flag))
            {
                w.WriteLine("1");
            }
            serverOrLite = true;
            StaticResouses.dBQ = new ServerQ();

            return "ok";
        }

        public static string NewConnectionLite()
        {
            SqliteCommand sqlCommand;

            try
            {
                tempLite = new SqliteConnection("DataSource = "+ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\local.db"));
                tempLite.Open();
                sqlCommand = new SqliteCommand(GetTablesLite(), tempLite);
                sqlCommand.ExecuteNonQuery();
                tempLite.Close();
            }
            catch(Exception e)
            {
                return e.Message;
            }

            connection = tempLite;

            using (StreamWriter w = new(flag))
            {
                w.WriteLine("0");
            }
            serverOrLite = false;
            StaticResouses.dBQ = new LiteQ();

            return "ok";
        }

        public static List<T> Read<T>(string query, Func<IDataReader, T> mapper)
        {
            List<T> result = new();
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(mapper(reader));
                }
            }
            connection.Close();
            return result;
        }

        public static List<T> ReadFull<T>(string query, Func<IDataReader, T> mapper)
        {
            List<T> result = new();
            if (connection.State == ConnectionState.Closed) connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(mapper(reader));
                }
            }
            return result;
        }

        public static bool Execute(string query, params IDbDataParameter[] parameters)
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            AddParameters(command, parameters);
            bool result = command.ExecuteNonQuery() > 0;
            connection.Close();
            return result;
        }

        public static bool ExecuteFull(string query, params IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            AddParameters(command, parameters);
            return command.ExecuteNonQuery() > 0;
        }

        private static void AddParameters(IDbCommand command, IDbDataParameter[] parameters)
        {
            if (parameters == null && parameters.Length == 0) return;
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }
        }

        public static bool ConnChangeTemp(string str)
        {
            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = str;
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}