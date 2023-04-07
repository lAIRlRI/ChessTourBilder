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
    internal class TourControler
    {
        public static Tour nowTour = new();
        static List<Tour> models;
        private static List<IDbDataParameter> list;
        private static readonly Func<IDataReader, Tour> mapper = r => new Tour()
        {
            TourID = Convert.ToInt32(r["TourID"]),
            NameTour = r["NameTour"].ToString(),
            EventID = Convert.ToInt32(r["EventID"])
        };


        private static void SqlParameterSet(Tour model)
        {
            list = DataBase.SetParameters
                (
                    new List<ParametrBD>()
                    {
                        new ParametrBD("@EventID",model.EventID),
                        new ParametrBD("@NameTour",model.NameTour)
                    }
                );
        }

        public static bool Insert(Tour model)
        {
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO [dbo].[Tour]([EventID],[NameTour])" +
                                                          "VALUES(@EventID,@NameTour)", list.ToArray());
        }

        public static bool Update(Tour model)
        {
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE [dbo].[Tour] " +
                $"SET [EventID] = @EventID" +
                $",[NameTour] = @NameTour" +
                $" WHERE ID = {model.TourID}", list.ToArray());
        }

        public static bool Delete(int id) => DataBase.Execute($"DELETE FROM [dbo].[Tour] WHERE TourID = {id}");

        public static List<Tour> Get(string str)
        {
            models = DataBase.Read(str, mapper);
            return models;
        }

        public static List<Tour> Get()
        {
            models = DataBase.Read("SELECT * FROM Tour", mapper);
            return models;
        }

        public static Tour Get(int id)
        {
            models = DataBase.Read($"SELECT * FROM Tour WHERE TourID = {id}", mapper);
            return models[0];
        }

        public static Tour GetLast()
        {
            models = DataBase.Read("SELECT * FROM Tour where TourID = (select max(TourID) from Tour)", mapper);
            return models[0];
        }
    }
}