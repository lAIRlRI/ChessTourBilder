namespace MauiApp3.Data;

using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

public class DataBaseFullConn
{
    static string db = "CTB_version_2_2_20230204";
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

    public static DataSet ConnDataSet(string str)
    {
        try
        {
            SqlDataAdapter adapter = new SqlDataAdapter(str, sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
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
            SqlCommand sqlCommand = new SqlCommand(str, sqlConnection);
            bool result = sqlCommand.ExecuteNonQuery() > 0;
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static void OpenConn() 
    {
        sqlConnection.Open();
    }

    public static void CloseCon()
    {
        sqlConnection.Close();
    }
}