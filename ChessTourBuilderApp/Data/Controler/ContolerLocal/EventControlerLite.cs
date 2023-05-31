using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class EventControlerLite : IEventControler
    {
        public Event nowEvent { get; set; } = new();
        static List<Event> models;
        static List<IDbDataParameter> list;
        public static Func<IDataReader, Event> mapper = r => new Event()
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

        public async Task<bool> Insert(Event model)
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
            DataBase.Execute($"create table {(await GetLast()).GetTableName()} (" +
                                                       "EventID int not null," +
                                                       "PlayerID int not null," +
                                                       "Result float not null," +
                                                       "ConsignmentID int not null)");
            return true;
        }

        public async Task<bool> Update(Event model, int id)
        {
            await Task.Delay(2);
            SqlParameterSet(model);
            DataBase.Execute($"UPDATE Event " +
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
            return true;
        }

        public async Task<bool> UpdateStatus()
        {
            await Task.Delay(2);
            DataBase.Execute("UPDATE Event SET StatusID = CASE " +
               "WHEN DataStart > date('now') THEN 2 " +
               "WHEN DataFinish > date('now') AND DataStart < date('now') THEN 3 " +
               "ELSE 1 END;");
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            await Task.Delay(2);
            DataBase.Execute($"DELETE FROM Event WHERE EventID = {id}");
            return true;
        }

        public async Task<List<Event>> GetAll()
        {
            await Task.Delay(2);
            models = DataBase.Read("SELECT * FROM Event", mapper);
            return models;
        }

        public async Task<List<Event>> GetPublic()
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM Event WHERE IsPublic = 1 OR OrganizerId = {StaticResouses.mainControler.OrganizerControler.nowOrganizer.OrganizerID};", mapper);
            return models;
        }

        public async Task<List<Event>> GetPlayerEvent()
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT e.* FROM Event e WHERE EXISTS (SELECT 1 FROM EventPlayer ep WHERE ep.EventId = e.EventId AND ep.PlayerId = {StaticResouses.mainControler.PlayerControler.nowPlayer.FIDEID});", mapper);
            return models;
        }

        public async Task<Event> GetById(int id)
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM Event WHERE EventID = {id}", mapper);
            return models[0];
        }

        public async Task<Event> GetLast()
        {
            await Task.Delay(2);
            models = DataBase.Read("SELECT * FROM Event where EventID = (select max(EventID) from Event)", mapper);
            return models[0];
        }
    }
}