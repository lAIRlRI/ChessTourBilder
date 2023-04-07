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
    internal class EventPlayerControler
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

        public static bool Insert(EventPlayer model)
        {
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO [dbo].[EventPlayer]([EventID],[PlayerID],[TopPlece])" +
                                                          "VALUES(@EventID,@PlayerID,@TopPlece)", list.ToArray());
        }

        public static bool Update(EventPlayer model)
        {
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE [dbo].[EventPlayer] " +
                $"SET [EventID] = @EventID" +
                $",[PlayerID] = @PlayerID" +
                $",[TopPlece] = @TopPlece" +
                $" WHERE ID = {model.EventPlayerID}", list.ToArray());
        }

        public static bool Delete(int id) => DataBase.Execute($"DELETE FROM [dbo].[EventPlayer] WHERE EventPlayerID = {id}");

        public static List<EventPlayer> Get(string str)
        {
            models = DataBase.Read(str, mapper);
            return models;
        }

        public static List<EventPlayer> Get()
        {
            models = DataBase.Read("SELECT * FROM EventPlayer", mapper);
            return models;
        }

        public static EventPlayer Get(int id)
        {
            models = DataBase.Read($"SELECT * FROM EventPlayer WHERE EventPlayerID = {id}", mapper);
            return models[0];
        }
    }
}