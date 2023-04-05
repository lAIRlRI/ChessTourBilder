using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class EventControler
    {
        public static Event nowEvent = new();
        static List<Event> events;
        static SqlDataReader reader;
        public static List<Event> staticEvent;
        static List<SqlParameter> list = new()
                                        {
                                            new SqlParameter() {ParameterName = "@Name" },
                                            new SqlParameter() {ParameterName = "@PrizeFund" },
                                            new SqlParameter() {ParameterName = "@DataStart" },
                                            new SqlParameter() {ParameterName = "@DataFinish" },
                                            new SqlParameter() {ParameterName = "@StatusID" },
                                            new SqlParameter() {ParameterName = "@OrganizerID" },
                                            new SqlParameter() {ParameterName = "@LocationEvent" },
                                            new SqlParameter() {ParameterName = "@TypeEvent" },
                                            new SqlParameter() {ParameterName = "@IsPublic" }
                                        };


        private static void SqlParameterSet(Event model)
        {
            list[0].Value = model.Name;
            list[1].Value = model.PrizeFund;
            list[2].Value = model.DataStart;
            list[3].Value = model.DataFinish;
            list[4].Value = model.StatusID;
            list[5].Value = model.OrganizerID;
            list[6].Value = model.LocationEvent;
            list[7].Value = model.TypeEvent;
            list[8].Value = model.IsPublic;
        }

        public EventControler() => staticEvent = Get();

        public static bool Insert(Event model)
        {
            SqlParameterSet(model);
            DataBase.ConnChange("INSERT INTO [dbo].[Event](" +
                                                                "[Name]," +
                                                                "[PrizeFund]," +
                                                                "[DataStart]," +
                                                                "[DataFinish]," +
                                                                "[StatusID]," +
                                                                "[OrganizerID]," +
                                                                "[LocationEvent]," +
                                                                "[TypeEvent]," +
                                                                "[IsPublic])" +
                                                          "VALUES(" +
                                                                $"@Name," +
                                                                $"@PrizeFund," +
                                                                $"@DataStart," +
                                                                $"@DataFinish," +
                                                                $"@StatusID," +
                                                                $"@OrganizerID," +
                                                                $"@LocationEvent," +
                                                                $"@TypeEvent," +
                                                                $"@IsPublic)", list);
            DataBase.ConnChange($"create table {GetLast().GetTableName()} (" +
                                                       "EventID int not null," +
                                                       "PlayerID int not null," +
                                                       "Result float not null," +
                                                       "ConsignmentID int not null)");
            return true;
        }

        public static bool Update(Event model)
        {
            SqlParameterSet(model);
            return DataBase.ConnChange($"UPDATE [dbo].[Event] " +
                "SET[Name] = @Name" +
                ",[PrizeFund] = @PrizeFund" +
                ",[DataStart] = @DataStart" +
                ",[DataFinish] = @DataFinish" +
                ",[StatusID] = @StatusID" +
                ",[OrganizerID] = @OrganizerID" +
                ",[IsPublic] = @IsPublic" +
                ",[LocationEvent] = @LocationEvent" +
                ",[TypeEvent] = @TypeEvent" +
                " WHERE EventID = {model.EventID}", list);
        }

        public static bool Delete(Event model)
        {
            return DataBase.ConnChange($"DELETE FROM [dbo].[Event] WHERE EventID = {model.EventID}");
        }

        public static List<Event> Get(string str)
        {
            reader = DataBase.Conn(str);
            Reader();
            return events;
        }

        public static List<Event> Get()
        {
            reader = DataBase.Conn("SELECT * FROM Event");
            Reader();
            return events;
        }

        public static Event GetLast()
        {
            reader = DataBase.Conn("SELECT * FROM Event where EventID = (select max(EventID) from Event)");
            Reader();
            return events[0];
        }

        public static Event Get(int id)
        {
            reader = DataBase.Conn($"SELECT * FROM Event WHERE EventID = {id}");
            Reader();
            return events[0];
        }

        private static void Reader()
        {
            events = new List<Event>();
            while (reader.Read())
            {
                events.Add(
                    new Event()
                    {
                        EventID = Convert.ToInt32(reader["EventID"]),
                        Name = reader["Name"].ToString(),
                        PrizeFund = Convert.ToInt32(reader["PrizeFund"]),
                        LocationEvent = reader["LocationEvent"].ToString(),
                        DataStart = Convert.ToDateTime(reader["DataStart"]),
                        DataFinish = Convert.ToDateTime(reader["DataFinish"]),
                        StatusID = Convert.ToInt32(reader["StatusID"]),
                        OrganizerID = Convert.ToInt32(reader["OrganizerID"]),
                        TypeEvent = Convert.ToBoolean(reader["TypeEvent"]),
                        IsPublic = Convert.ToBoolean(reader["IsPublic"])
                    }
                );
            }
            reader.Close();
            DataBase.CloseCon();
        }

    }
}
