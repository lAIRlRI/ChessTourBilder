namespace MauiApp3.Data.Controler;

using MauiApp3.Data.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

internal class ConsignmentPlayerControler
{
    //public static ConsignmentPlayer nowConsignmentPlayer;
    static List<ConsignmentPlayer> models;
    static SqlDataReader reader;
    static List<SqlParameter> list = new List<SqlParameter>()
                                        {
                                            new SqlParameter() {ParameterName = "@ConsignmentID" },
                                            new SqlParameter() {ParameterName = "@PlayerID" },
                                            new SqlParameter() {ParameterName = "@IsWhile" },
                                            new SqlParameter() {ParameterName = "@Result" }
                                        };


    private static void SqlParameterSet(ConsignmentPlayer model)
    {
        list[0].Value = model.ConsignmentID;
        list[1].Value = model.PlayerID;
        list[2].Value = model.IsWhile;
        list[3].Value = model.Result == null ? DBNull.Value : model.Result;
    }

    public static bool Insert(ConsignmentPlayer model)
    {
        SqlParameterSet(model);
        return DataBase.ConnChange("INSERT INTO [dbo].[ConsignmentPlayer](" +
                                                            "[ConsignmentID]," +
                                                            "[PlayerID]," +
                                                            "[IsWhile]," +
                                                            "[Result])" +
                                                      "VALUES(" +
                                                            $"@ConsignmentID," +
                                                            $"@PlayerID," +
                                                            $"@IsWhile," +
                                                            $"@Result)", list);
    }

    public static bool Update(ConsignmentPlayer model)
    {
        SqlParameterSet(model);
        if(!DataBase.ConnChange($"UPDATE [dbo].[ConsignmentPlayer] " +
            $"SET [ConsignmentID] = @ConsignmentID" +
            $",[PlayerID] = @PlayerID" +
            $",[IsWhile] = @IsWhile" +
            $",[Result] = @Result"+
            $" WHERE ConsignmentPlayerID = {model.ConsignmentPlayerID}", list)) return false;
        if (!PlayerControler.Update(model.player, model.PlayerID)) return false;
        return true;

    }

    public static bool Delete(int id) => DataBase.ConnChange($"DELETE FROM [dbo].[ConsignmentPlayer] WHERE ConsignmentPlayerID = {id}");

    public static List<ConsignmentPlayer> Get(string str)
    {
        reader = DataBase.Conn(str);
        Reader();
        ConsignmentPlayerPlayerGet();
        return models;
    }

    public static List<ConsignmentPlayer> Get()
    {
        reader = DataBase.Conn("select * from ConsignmentPlayer" );
        Reader();
        ConsignmentPlayerPlayerGet();
        return models;
    }

    private static void ConsignmentPlayerPlayerGet()
    {
        foreach (var item in models)
        {
            item.player = PlayerControler.staticPlayer.Where(p => p.FIDEID == item.PlayerID).FirstOrDefault();
        }
    }

    private static void Reader()
    {       
        models = new List<ConsignmentPlayer>();
        while (reader.Read())
        {
            if (reader.IsDBNull(0))
            {
                reader.Close();
                DataBase.CloseCon();
                return;
            }
            models.Add(
                new ConsignmentPlayer()
                {
                    ConsignmentPlayerID = Convert.ToInt32(reader["ConsignmentPlayerID"]),
                    ConsignmentID = Convert.ToInt32(reader["ConsignmentID"]),
                    PlayerID = Convert.ToInt32(reader["PlayerID"]),
                    IsWhile = Convert.ToBoolean(reader["IsWhile"]),
                    Result = reader.IsDBNull(reader.GetOrdinal("Result")) ?
                                                            null : Convert.ToDouble(reader["Result"])
                }
            );
        }
        reader.Close();
        DataBase.CloseCon();
    }
}
