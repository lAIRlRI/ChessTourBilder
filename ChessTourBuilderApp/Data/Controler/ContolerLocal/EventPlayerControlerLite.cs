using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class EventPlayerControlerLite : IEventPlayerControler
    {
        public static EventPlayer nowEventPlayer = new();

        static List<EventPlayer> models;
        private static List<IDbDataParameter> list;
        private static readonly Func<IDataReader, EventPlayer> mapper = r => new EventPlayer()
        {
            EventPlayerID = Convert.ToInt32(r["EventPlayerID"]),
            PlayerID = Convert.ToInt32(r["PlayerID"]),
            EventID = Convert.ToInt32(r["EventID"]),
            TopPlece = r.IsDBNull(r.GetOrdinal("TopPlece")) ? null : Convert.ToInt32(r["TopPlece"])
        };

        private static void SqlParameterSet(EventPlayer model)
        {
            list = DataBase.SetParameters
                 (
                     new List<ParametrBD>()
                     {
                        new ParametrBD("@EventID", model.EventID),
                        new ParametrBD("@PlayerID",model.PlayerID),
                        new ParametrBD("@TopPlece", model.TopPlece == null ? DBNull.Value : model.TopPlece)
                     }
                 );
        }

        public async Task<bool> Insert(EventPlayer model)
        {
            await Task.Delay(2);
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO EventPlayer (EventID,PlayerID,TopPlece)" +
                                                          "VALUES(@EventID,@PlayerID,@TopPlece)", list.ToArray());
        }

        public async Task<bool> Update(EventPlayer model, int id)
        {
            await Task.Delay(2);
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE EventPlayer " +
                $"SET EventID = @EventID" +
                $",PlayerID = @PlayerID" +
                $",TopPlece = @TopPlece" +
                $" WHERE ID = {model.EventPlayerID}", list.ToArray());
        }

        public async Task<bool> Delete(int id)
        {
            await Task.Delay(2);
            return DataBase.Execute($"DELETE FROM EventPlayer WHERE EventPlayerID = {id}");
        }

        public async Task<List<EventPlayer>> GetAll() 
        {
            await Task.Delay(2);
            models = DataBase.Read("SELECT * FROM EventPlayer", mapper);
            return models;
        }

        public async Task<EventPlayer> GetById(int id) 
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM EventPlayer WHERE EventPlayerID = {id}", mapper);
            return models[0];
        }
    }
}