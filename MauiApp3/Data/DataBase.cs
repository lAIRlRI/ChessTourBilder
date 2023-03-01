namespace MauiApp3.Data;

using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

public class DataBase
{
    static string db = "CTB_version_2_4_20230301";
    static string ip = "DESKTOP-7T6ACSF\\SQLEXPRESS";
    static string userName = "ARR";
    static string userPassword = "1234";
    static string sqlcon = $"Data Source = {ip}; " +
                           $"Initial Catalog = {db}; " +
                           $"User ID = {userName};" +
                           $"Password = {userPassword};" +
                           $"Trusted_Connection = true;" +
                           $"TrustServerCertificate = true;" +
                           $"Encrypt = false;" +
                           $"Integrated Security = true;";
    static SqlConnection sqlConnection = new SqlConnection(sqlcon);
    static SqlCommand sqlCommand;
    static SqlDataReader reader;

    public static SqlDataReader Conn(string str)
    {
        try
        {
            OpenConn(str);
            reader = sqlCommand.ExecuteReader();
            return reader;
        }
        catch(Exception e)
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
}