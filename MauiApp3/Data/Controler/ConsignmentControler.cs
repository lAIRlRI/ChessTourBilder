namespace MauiApp3.Data.Controler;

using MauiApp3.Data.Model;
using MauiApp3.Pages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

internal class ConsignmentControler
{
    public static Consignment nowConsignment;
    static List<Consignment> models;
    static SqlDataReader reader;
    static List<SqlParameter> list = new List<SqlParameter>()
                                        {
                                            new SqlParameter() {ParameterName = "@TourID" },
                                            new SqlParameter() {ParameterName = "@StatusID" },
                                            new SqlParameter() {ParameterName = "@DateStart" }
                                        };


    private static void SqlParameterSet(Consignment model)
    {
        list[0].Value = model.TourID;
        list[1].Value = model.StatusID;
        list[2].Value = model.DateStart;
    }

    public static bool Insert(Consignment model)
    {

        SqlParameterSet(model);
        if (!DataBase.ConnChange("INSERT INTO [dbo].[Consignment](" +
                                                            "[TourID]," +
                                                            "[StatusID]," +
                                                            "[DateStart])" +
                                                      "VALUES(" +
                                                            $"@TourID," +
                                                            $"@StatusID," +
                                                            $"@DateStart)", list)) return false;
        int modelID = Get().Max(p => p.ConsignmentID);
        model.whitePlayer.ConsignmentID = modelID;
        model.whitePlayer.IsWhile = true;

        model.blackPlayer.ConsignmentID = modelID;
        model.blackPlayer.IsWhile = false;

        if (!ConsignmentPlayerControler.Insert(model.whitePlayer)) return false;
        if (!ConsignmentPlayerControler.Insert(model.blackPlayer)) return false;

        return true;
    }

    public static bool Update(Consignment model)
    {
        SqlParameterSet(model);
        return DataBase.ConnChange($"UPDATE [dbo].[Consignment] " +
            $"SET [TourID ] = @TourID" +
            $",[StatusID] = @StatusID" +
            $",[DateStart] = @DateStart" +
            $" WHERE ConsignmentID = {model.ConsignmentID}", list);
    }

    public static bool Delete(int id) => DataBase.ConnChange($"DELETE FROM [dbo].[Consignment] WHERE ConsignmentID = {id}");

    public static List<Consignment> Get(string str)
    {
        reader = DataBase.Conn(str);
        Reader();
        ConsignmentPlayerGet();
        return models;
    }

    public static Consignment GetLast()
    {
        reader = DataBase.Conn("SELECT * FROM Consignment where ConsignmentID = (select max(ConsignmentID) from Consignment)");
        Reader();
        ConsignmentPlayerGet();
        return models[0];
    }

    public static List<Consignment> Get()
    {
        reader = DataBase.Conn("select * from Consignment");
        Reader();
        ConsignmentPlayerGet();
        return models;
    }

    private static void ConsignmentPlayerGet()
    {
        foreach (var item in models)
        {
            List<ConsignmentPlayer> consignmentPlayers = ConsignmentPlayerControler.Get($"select * from ConsignmentPlayer where ConsignmentID = {item.ConsignmentID}").ToList();
            if (consignmentPlayers.Count == 0) return;
            if (consignmentPlayers[0].IsWhile)
            {
                item.whitePlayer = consignmentPlayers[0];
                item.blackPlayer = consignmentPlayers[1];
            }
            else
            {
                item.whitePlayer = consignmentPlayers[1];
                item.blackPlayer = consignmentPlayers[0];
            }
        }
    }

    private static void Reader()
    {
        models = new List<Consignment>();
        while (reader.Read())
        {
            if (reader.IsDBNull(0))
            {
                reader.Close();
                DataBase.CloseCon();
                return;
            }
            models.Add(
                new Consignment()
                {
                    ConsignmentID = Convert.ToInt32(reader["ConsignmentID"]),
                    TourID = Convert.ToInt32(reader["TourID"]),
                    StatusID = Convert.ToInt32(reader["StatusID"]),
                    DateStart = Convert.ToDateTime(reader["DateStart"])
                }
            );
        }
        reader.Close();
        DataBase.CloseCon();
    }
}