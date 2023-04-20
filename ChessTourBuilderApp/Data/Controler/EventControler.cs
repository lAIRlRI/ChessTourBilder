using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class EventControler
    {
        public static Event nowEvent = new();
        static List<Event> models;
        static List<IDbDataParameter> list;
        static Func<IDataReader, Event> mapper = r => new Event()
            {
                EventID = Convert.ToInt32(r["EventID"]),
                Name = r["Name"].ToString(),
                PrizeFund = Convert.ToInt32(r["PrizeFund"]),
                LocationEvent = r["LocationEvent"].ToString(),
                DataStart = Convert.ToDateTime(r["DataStart"]),
                DataFinish = Convert.ToDateTime(r["DataFinish"]),
                StatusID = Convert.ToInt32(r["StatusID"]),
                OrganizerID = Convert.ToInt32(r["OrganizerID"]),
                TypeEvent = Convert.ToBoolean(r["TypeEvent"]),
                IsPublic = Convert.ToBoolean(r["IsPublic"])
            };

        private static void SqlParameterSet(Event model)
        {
            list = DataBase.SetParameters
                (
                    new List<ParametrBD>() 
                    {
                        new ParametrBD("@Name",model.Name),
                        new ParametrBD("@PrizeFund",model.PrizeFund),
                        new ParametrBD("@DataStart",model.DataStart),
                        new ParametrBD("@DataFinish",model.DataFinish),
                        new ParametrBD("@StatusID",model.StatusID),
                        new ParametrBD("@OrganizerID",model.OrganizerID),
                        new ParametrBD("@LocationEvent",model.LocationEvent),
                        new ParametrBD("@TypeEvent",model.TypeEvent),
                        new ParametrBD("@IsPublic",model.IsPublic),
                    }
                );
        }

        public static bool Insert(Event model)
        {
            SqlParameterSet(model);
            DataBase.Execute("INSERT INTO Event(" +
                                                                "Name," +
                                                                "PrizeFund," +
                                                                "DataStart," +
                                                                "DataFinish," +
                                                                "StatusID," +
                                                                "OrganizerID," +
                                                                "LocationEvent," +
                                                                "TypeEvent," +
                                                                "IsPublic)" +
                                                          "VALUES(" +
                                                                $"@Name," +
                                                                $"@PrizeFund," +
                                                                $"@DataStart," +
                                                                $"@DataFinish," +
                                                                $"@StatusID," +
                                                                $"@OrganizerID," +
                                                                $"@LocationEvent," +
                                                                $"@TypeEvent," +
                                                                $"@IsPublic)", list.ToArray());
            DataBase.Execute($"create table {GetLast().GetTableName()} (" +
                                                       "EventID int not null," +
                                                       "PlayerID int not null," +
                                                       "Result float not null," +
                                                       "ConsignmentID int not null)");
            return true;
        }

        public static bool Update(Event model)
        {
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE Event " +
                "SET Name = @Name" +
                ",PrizeFund = @PrizeFund" +
                ",DataStart = @DataStart" +
                ",DataFinish = @DataFinish" +
                ",StatusID = @StatusID" +
                ",OrganizerID = @OrganizerID" +
                ",IsPublic = @IsPublic" +
                ",LocationEvent = @LocationEvent" +
                ",TypeEvent = @TypeEvent" +
                $" WHERE EventID = {model.EventID}", list.ToArray());
        }

        public static bool Delete(Event model)
        {
            return DataBase.Execute($"DELETE FROM Event WHERE EventID = {model.EventID}");
        }

        public static List<Event> Get(string str)
        {
            models = DataBase.Read(str, mapper);
            return models;
        }

        public static List<Event> Get()
        {
            models = DataBase.Read("SELECT * FROM Event", mapper);
            return models;
        }

        public static Event GetLast()
        {
            models = DataBase.Read("SELECT * FROM Event where EventID = (select max(EventID) from Event)", mapper);
            return models[0];
        }

        public static Event Get(int id)
        {
            models = DataBase.Read($"SELECT * FROM Event WHERE EventID = {id}", mapper);
            return models[0];
        }
    }
}
