namespace MauiApp3.Data.Controler;

using MauiApp3.Data.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

internal class EventPlayerControler
{
    public static EventPlayer nowEventPlayer = new();
    static List<EventPlayer> events;
    static SqlDataReader reader;
    static List<SqlParameter> list = new List<SqlParameter>()
                                        {
                                            new SqlParameter() {ParameterName = "@EventID" },
                                            new SqlParameter() {ParameterName = "@PlayerID" },
                                            new SqlParameter() {ParameterName = "@TopPlece" }
                                        };


    private static void SqlParameterSet(EventPlayer model)
    {
        list[0].Value = model.EventID;
        list[1].Value = model.PlayerID;
        list[2].Value = model.TopPlece == null ? DBNull.Value: model.TopPlece;
    }

    public static bool Insert(EventPlayer model)
    {
        SqlParameterSet(model);
        return DataBase.ConnChange("INSERT INTO [dbo].[EventPlayer]([EventID],[PlayerID],[TopPlece])" +
                                                      "VALUES(@EventID,@PlayerID,@TopPlece)", list);
    }

    public static bool Update(EventPlayer model)
    {
        SqlParameterSet(model);
        return DataBase.ConnChange($"UPDATE [dbo].[EventPlayer] " +
            $"SET [EventID] = @EventID" +
            $",[PlayerID] = @PlayerID" +
            $",[TopPlece] = @TopPlece" +
            $" WHERE ID = {model.EventPlayerID}", list);
    }

    public static bool Delete(int id) => DataBase.ConnChange($"DELETE FROM [dbo].[EventPlayer] WHERE EventPlayerID = {id}");

    public static List<EventPlayer> Get(string str)
    {
        reader = DataBase.Conn(str);
        Reader();
        return events;
    }

    public static List<EventPlayer> Get()
    {
        reader = DataBase.Conn("SELECT * FROM EventPlayer");
        Reader();
        return events;
    }

    public static EventPlayer Get(int id)
    {
        reader = DataBase.Conn($"SELECT * FROM EventPlayer WHERE EventPlayerID = {id}");
        Reader();
        return events[0];
    }

    private static void Reader()
    {
        events = new List<EventPlayer>();
        while (reader.Read())
        {
            events.Add(
                new EventPlayer()
                {
                    EventPlayerID = Convert.ToInt32(reader["EventPlayerID"]),
                    PlayerID = Convert.ToInt32(reader["PlayerID"]),
                    EventID = Convert.ToInt32(reader["EventID"]),
                    TopPlece = reader.IsDBNull(reader.GetOrdinal("TopPlece")) ?
                                                            null : Convert.ToInt32(reader["TopPlece"])
                }
            );
        }
        reader.Close();
        DataBase.CloseCon();
    }
}
