namespace MauiApp3.Data;

using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class DataBase
{
    static string db;
    static string ip;
    static string userName;
    static string userPassword;
    static string sqlcon;
    static string paths;
    static SqlConnection sqlConnection;
    static SqlCommand sqlCommand;
    static SqlDataReader reader;
    public static SqlConnection temp;

    public static DataBaseFullConn DataBaseFullConn = new DataBaseFullConn();

    public DataBase()
    {
        paths = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\server.txt");
        string[] lines = File.ReadAllLines(paths);
        if (lines.Length == 4)
        {
            ip = lines[0];
            db = lines[1];
            userName = lines[2];
            userPassword = lines[3];
        }
        sqlcon = $"Data Source = {ip}; " +
                              $"Initial Catalog = {db}; " +
                              $"User ID = {userName};" +
                              $"Password = {userPassword};" +
                              $"Trusted_Connection = true;" +
                              $"TrustServerCertificate = true;" +
                              $"Encrypt = false;" +
                              $"Integrated Security = true;";
        sqlConnection = new SqlConnection(sqlcon);
    }

    public static SqlDataReader Conn(string str)
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

    public static DataSet ConnDataSet(string str)
    {
        try
        {
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(str, sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            CloseCon();
            return ds;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static bool ConnChangeTemp(string str)
    {
        try
        {
           
            temp.Open();
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

    public static bool ConnChange(string str)
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

    public static bool ConnChange(string str, List<SqlParameter> list)
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

    private static void OpenConn(string str)
    {
        sqlConnection.Open();
        sqlCommand = new SqlCommand(str, sqlConnection);
    }

    private static void OpenConn(string str, List<SqlParameter> list)
    {
        sqlConnection.Open();
        sqlCommand = new SqlCommand(str, sqlConnection);
        foreach (var item in list)
        {
            sqlCommand.Parameters.Add(item);
        }
    }

    public static void CloseCon()
    {
        sqlCommand.Parameters.Clear();
        sqlConnection.Close();
    }

    public static bool ChangeConnection()
    {
        try
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select 1 from Organizer", sqlConnection);
            bool result = sqlCommand.ExecuteNonQuery() > 0;
            sqlConnection.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string NewConnection(string[] values)
    {
        
        SqlCommand sqlCommand;

        try
        {
            temp = new SqlConnection($"Data Source = {values[0]}; " +
                          $"Initial Catalog = master; " +
                          $"User ID = {values[2]};" +
                          $"Password = {values[3]};" +
                          $"Trusted_Connection = true;" +
                          $"TrustServerCertificate = true;" +
                          $"Encrypt = false;" +
                          $"Integrated Security = true;");
            temp.Open();
            sqlCommand = new SqlCommand("select 1", temp);
            sqlCommand.ExecuteNonQuery();
        }
        catch
        {
            return "NoConn";

        }

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
        }
        catch
        {
            return "NoDB";
        }

        try
        {
            sqlCommand = new SqlCommand($"select 1 from Organizer", temp);
            sqlCommand.ExecuteNonQuery();
        }
        catch
        {
            return "NoTable";
        }

        temp.Close();
        sqlConnection = temp;

        using (StreamWriter w = new StreamWriter(paths))
        {
            w.WriteLine(values[0]);
            w.WriteLine(values[1]);
            w.WriteLine(values[2]);
            w.WriteLine(values[3]);
        }

        return "ok";
    }
}