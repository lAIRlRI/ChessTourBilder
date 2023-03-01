namespace MauiApp3.Data.Controler;

using MauiApp3.Data.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

internal class TourControler
{
    public static Tour nowTour = new();
    static List<Tour> models;
    static SqlDataReader reader;
    static List<SqlParameter> list = new List<SqlParameter>()
                                        {
                                            new SqlParameter() {ParameterName = "@EventID" },
                                            new SqlParameter() {ParameterName = "@NameTour" }
                                        };


    private static void SqlParameterSet(Tour model)
    {
        list[0].Value = model.EventID;
        list[1].Value = model.NameTour;
    }

    public static bool Insert(Tour model)
    {
        SqlParameterSet(model);
        return DataBase.ConnChange("INSERT INTO [dbo].[Tour]([EventID],[NameTour])" +
                                                      "VALUES(@EventID,@NameTour)", list);
    }

    public static bool Update(Tour model)
    {
        SqlParameterSet(model);
        return DataBase.ConnChange($"UPDATE [dbo].[Tour] " +
            $"SET [EventID] = @EventID" +
            $",[NameTour] = @NameTour" +
            $" WHERE ID = {model.TourID}", list);
    }

    public static bool Delete(int id) => DataBase.ConnChange($"DELETE FROM [dbo].[Tour] WHERE TourID = {id}");

    public static List<Tour> Get(string str)
    {
        reader = DataBase.Conn(str);
        Reader();
        return models;
    }

    public static List<Tour> Get()
    {
        reader = DataBase.Conn("SELECT * FROM Tour");
        Reader();
        return models;
    }

    public static Tour Get(int id)
    {
        reader = DataBase.Conn($"SELECT * FROM Tour WHERE TourID = {id}");
        Reader();
        return models[0];
    }

    public static Tour GetLast()
    {
        reader = DataBase.Conn("SELECT * FROM Tour where TourID = (select max(TourID) from Tour)");
        Reader();
        return models[0];
    }

    private static void Reader()
    {
        models = new List<Tour>();
        while (reader.Read())
        {
            models.Add(
                new Tour()
                {
                    TourID = Convert.ToInt32(reader["TourID"]),
                    NameTour = reader["NameTour"].ToString(),
                    EventID = Convert.ToInt32(reader["EventID"])
                }
            );
        }
        reader.Close();
        DataBase.CloseCon();
    }
}
