using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class TourControler
    {
        public static Tour nowTour = new();
        static List<Tour> models;
        static SqlDataReader reader;
        static List<SqlParameter> list = new()
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
            return StaticResouses.dataBase.ConnChange("INSERT INTO [dbo].[Tour]([EventID],[NameTour])" +
                                                          "VALUES(@EventID,@NameTour)", list);
        }

        public static bool Update(Tour model)
        {
            SqlParameterSet(model);
            return StaticResouses.dataBase.ConnChange($"UPDATE [dbo].[Tour] " +
                $"SET [EventID] = @EventID" +
                $",[NameTour] = @NameTour" +
                $" WHERE ID = {model.TourID}", list);
        }

        public static bool Delete(int id) => StaticResouses.dataBase.ConnChange($"DELETE FROM [dbo].[Tour] WHERE TourID = {id}");

        public static List<Tour> Get(string str)
        {
            reader = StaticResouses.dataBase.Conn(str);
            Reader();
            return models;
        }

        public static List<Tour> Get()
        {
            reader = StaticResouses.dataBase.Conn("SELECT * FROM Tour");
            Reader();
            return models;
        }

        public static Tour Get(int id)
        {
            reader = StaticResouses.dataBase.Conn($"SELECT * FROM Tour WHERE TourID = {id}");
            Reader();
            return models[0];
        }

        public static Tour GetLast()
        {
            reader = StaticResouses.dataBase.Conn("SELECT * FROM Tour where TourID = (select max(TourID) from Tour)");
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
            StaticResouses.dataBase.CloseCon();
        }
    }
}
