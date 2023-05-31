﻿using ChessTourBuilderApp.Data.Api;
using ChessTourBuilderApp.Data.HelpClasses;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Reflection;

namespace ChessTourBuilderApp.Data.DataBases
{
    internal class DataBase
    {
        static string pathsSQL;
        static string flag;
        public static IDbConnection connection;
        public static SqliteConnection tempLite;
        public static bool serverOrLite = true;

        public static List<IDbDataParameter> SetParameters(List<ParametrBD> parametrs)
        {
            List<IDbDataParameter> temp = new();

            foreach (var item in parametrs)
            {
                SqliteParameter sqlParameter = new()
                {
                    ParameterName = item.ParameterName,
                    Value = item.ParameterValue
                };
                temp.Add(sqlParameter);
            }
            return temp;
        }

        public async static  Task<string> GetFlag()
        {
                //using var stream = await FileSystem.OpenAppPackageFileAsync("FlagBD.txt");
                //using var reader = new StreamReader(stream);

                //var contents = reader.ReadToEnd();
                flag = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wwwroot/DataBases/FlagBD.txt");

            return File.ReadAllText(flag);
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

        public async static Task<bool> ChangeConnection()
        {
            try
            {
                string paths = Path.Combine(FileSystem.AppDataDirectory, @"Data\DataBases\serverSetting.txt");
                string[] lines = File.ReadAllLines(paths);

                ApiControler.baseURL = lines[0];

                StaticResouses.mainControler = new(true);
                await StaticResouses.mainControler.OrganizerControler.GetAll();

                using (StreamWriter w = new(flag))
                {
                    w.WriteLine("1");
                }

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
                StaticResouses.mainControler = new(false);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<string> NewConnection(string[] values)
        {
            try
            {
                StaticResouses.mainControler = new(true);
                ApiControler.baseURL = values[0];
                await StaticResouses.mainControler.OrganizerControler.GetAll();

                string paths = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\serverSetting.txt");

                using (StreamWriter w = new(paths))
                {
                    w.WriteLine(values[0]);
                }

                using (StreamWriter w = new(flag))
                {
                    w.WriteLine("1");
                }
                StaticResouses.mainControler = new(true);
            }
            catch
            {
                return "NoDB";
            }

            return "ok";
        }

        public static string NewConnectionLite()
        {
            SqliteCommand sqlCommand;

            try
            {
                tempLite = new SqliteConnection("DataSource = " + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\DataBases\local.db"));
                tempLite.Open();
                sqlCommand = new SqliteCommand(GetTablesLite(), tempLite);
                sqlCommand.ExecuteNonQuery();
                tempLite.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            connection = tempLite;

            using (StreamWriter w = new(flag))
            {
                w.WriteLine("0");
            }
            StaticResouses.mainControler = new(false);

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