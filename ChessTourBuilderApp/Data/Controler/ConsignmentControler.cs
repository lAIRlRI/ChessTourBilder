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
    internal class ConsignmentControler
    {
        public static Consignment nowConsignment;
        static List<Consignment> models;
        static List<IDbDataParameter> list;

        static Func<IDataReader, Consignment> mapper = r => new Consignment()
        {
            ConsignmentID = Convert.ToInt32(r["ConsignmentID"]),
            TourID = Convert.ToInt32(r["TourID"]),
            StatusID = Convert.ToInt32(r["StatusID"]),
            DateStart = Convert.ToDateTime(r["DateStart"]),
            GameMove = r["GameMove"].ToString()
        };


        private static void SqlParameterSet(Consignment model)
        {
            list = DataBase.SetParameters
                (
                    new List<ParametrBD>()
                    {
                        new ParametrBD( "@TourID", model.TourID ) ,
                        new ParametrBD("@StatusID", model.StatusID) ,
                        new ParametrBD("@DateStart", model.DateStart) ,
                        new ParametrBD("@GameMove", model.GameMove == null ? DBNull.Value : model.GameMove )
                    }
                );
        }

        public static bool Insert(Consignment model)
        {
            SqlParameterSet(model);
            if (!DataBase.Execute("INSERT INTO [dbo].[Consignment](" +
                                                                "[TourID]," +
                                                                "[StatusID]," +
                                                                "[DateStart])" +
                                                          "VALUES(" +
                                                                $"@TourID," +
                                                                $"@StatusID," +
                                                                $"@DateStart)", list.ToArray())) return false;
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
            if (!DataBase.Execute($"UPDATE [dbo].[Consignment] " +
                $"SET [TourID ] = @TourID" +
                $",[StatusID] = @StatusID" +
                $",[DateStart] = @DateStart" +
                $",[GameMove] = @GameMove" +
                $" WHERE ConsignmentID = {model.ConsignmentID}", list.ToArray())) return false;
            if (!ConsignmentPlayerControler.Update(model.blackPlayer)) return false;
            if (!ConsignmentPlayerControler.Update(model.whitePlayer)) return false;
            return true;
        }

        public static bool Delete(int id) => DataBase.Execute($"DELETE FROM [dbo].[Consignment] WHERE ConsignmentID = {id}");

        public static List<Consignment> Get(string str)
        {
            models = DataBase.Read(str, mapper);
            ConsignmentPlayerGet();
            return models;
        }

        public static Consignment GetLast()
        {
            models = DataBase.Read("SELECT * FROM Consignment where ConsignmentID = (select max(ConsignmentID) from Consignment)", mapper);
            ConsignmentPlayerGet();
            return models[0];
        }

        public static List<Consignment> Get()
        {
            models = DataBase.Read("SELECT * FROM Consignment", mapper);
            ConsignmentPlayerGet();
            return models;
        }

        private static void ConsignmentPlayerGet()
        {
            foreach (var item in models)
            {
                List<ConsignmentPlayer> consignmentPlayers = ConsignmentPlayerControler.Get($"select * from ConsignmentPlayer where ConsignmentID = {item.ConsignmentID}");
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
    }
}
