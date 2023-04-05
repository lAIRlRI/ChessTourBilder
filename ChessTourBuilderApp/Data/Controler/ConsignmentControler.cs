using ChessTourBuilderApp.Data.DataBases;
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
        static SqlDataReader reader;
        static List<SqlParameter> list = new()
                                        {
                                            new SqlParameter() {ParameterName = "@TourID" },
                                            new SqlParameter() {ParameterName = "@StatusID" },
                                            new SqlParameter() {ParameterName = "@DateStart" },
                                            new SqlParameter() {ParameterName = "@GameMove" }
                                        };


        private static void SqlParameterSet(Consignment model)
        {
            list[0].Value = model.TourID;
            list[1].Value = model.StatusID;
            list[2].Value = model.DateStart;
            list[3].Value = model.GameMove == null ? DBNull.Value : model.GameMove;
        }

        public static bool Insert(Consignment model)
        {
            SqlParameterSet(model);
            if (!DataBase.ConnChange("INSERT INTO [dbo].[Consignment](" +
                                                                "[TourID]," +
                                                                "[StatusID]," +
                                                                "[DateStart])" +
                                                          "VALUES(" +
                                                                $"@TourID," +
                                                                $"@StatusID," +
                                                                $"@DateStart)", list)) return false;
            int modelID = GetDataSet().Max(p => p.ConsignmentID);
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
            if (!DataBase.ConnChange($"UPDATE [dbo].[Consignment] " +
                $"SET [TourID ] = @TourID" +
                $",[StatusID] = @StatusID" +
                $",[DateStart] = @DateStart" +
                $",[GameMove] = @GameMove" +
                $" WHERE ConsignmentID = {model.ConsignmentID}", list)) return false;
            if (!ConsignmentPlayerControler.Update(model.blackPlayer)) return false;
            if (!ConsignmentPlayerControler.Update(model.whitePlayer)) return false;
            return true;
        }

        public static bool Delete(int id) => DataBase.ConnChange($"DELETE FROM [dbo].[Consignment] WHERE ConsignmentID = {id}");

        public static List<Consignment> Get(string str)
        {
            reader = DataBase.Conn(str);
            Reader();
            ConsignmentPlayerGet();
            return models;
        }

        public static Consignment GetLast()
        {
            DataSet ds = DataBase.ConnDataSet("SELECT * FROM Consignment where ConsignmentID = (select max(ConsignmentID) from Consignment)");
            DataSeter(ds);
            ConsignmentPlayerGet();
            return models[0];
        }

        public static List<Consignment> Get()
        {
            reader = DataBase.Conn("SELECT * FROM Consignment");
            Reader();
            ConsignmentPlayerGet();
            return models;
        }

        public static List<Consignment> GetDataSet()
        {
            DataSet ds = DataBase.ConnDataSet("SELECT * FROM Consignment");
            DataSeter(ds);
            ConsignmentPlayerGet();
            return models;
        }

        private static void ConsignmentPlayerGet()
        {
            foreach (var item in models)
            {
                List<ConsignmentPlayer> consignmentPlayers = ConsignmentPlayerControler.Get($"select * from ConsignmentPlayer where ConsignmentID = {item.ConsignmentID}").ToList();
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

        private static void Reader()
        {
            models = new List<Consignment>();
            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    reader.Close();
                    DataBase.CloseCon();
                    return;
                }
                models.Add(
                    new Consignment()
                    {
                        ConsignmentID = Convert.ToInt32(reader["ConsignmentID"]),
                        TourID = Convert.ToInt32(reader["TourID"]),
                        StatusID = Convert.ToInt32(reader["StatusID"]),
                        DateStart = Convert.ToDateTime(reader["DateStart"]),
                        GameMove = reader["GameMove"].ToString(),
                    }
                );
            }
            reader.Close();
            DataBase.CloseCon();
        }

        private static void DataSeter(DataSet set)
        {
            models = new List<Consignment>();
            foreach (DataRow item in set.Tables[0].Rows)
            {
                models.Add(
                    new Consignment()
                    {
                        ConsignmentID = Convert.ToInt32(item["ConsignmentID"]),
                        TourID = Convert.ToInt32(item["TourID"]),
                        StatusID = Convert.ToInt32(item["StatusID"]),
                        DateStart = Convert.ToDateTime(item["DateStart"]),
                        GameMove = item["GameMove"].ToString(),
                    }
                );
            }
        }
    }
}
