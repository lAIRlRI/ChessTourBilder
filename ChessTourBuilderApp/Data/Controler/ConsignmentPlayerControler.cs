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
    internal class ConsignmentPlayerControler
    {
        static List<ConsignmentPlayer> models;
        static SqlDataReader reader;
        static List<SqlParameter> list = new()
                                        {
                                            new SqlParameter() {ParameterName = "@ConsignmentID" },
                                            new SqlParameter() {ParameterName = "@PlayerID" },
                                            new SqlParameter() {ParameterName = "@IsWhile" },
                                            new SqlParameter() {ParameterName = "@Result" }
                                        };


        private static void SqlParameterSet(ConsignmentPlayer model)
        {
            list[0].Value = model.ConsignmentID;
            list[1].Value = model.PlayerID;
            list[2].Value = model.IsWhile;
            list[3].Value = model.Result == null ? DBNull.Value : model.Result;
        }

        public static bool Insert(ConsignmentPlayer model)
        {
            SqlParameterSet(model);
            return StaticResouses.dataBase.ConnChange("INSERT INTO [dbo].[ConsignmentPlayer](" +
                                                                "[ConsignmentID]," +
                                                                "[PlayerID]," +
                                                                "[IsWhile]," +
                                                                "[Result])" +
                                                          "VALUES(" +
                                                                $"@ConsignmentID," +
                                                                $"@PlayerID," +
                                                                $"@IsWhile," +
                                                                $"@Result)", list);
        }

        public static bool Update(ConsignmentPlayer model)
        {
            SqlParameterSet(model);
            if (!StaticResouses.dataBase.ConnChange($"UPDATE [dbo].[ConsignmentPlayer] " +
                $"SET [ConsignmentID] = @ConsignmentID" +
                $",[PlayerID] = @PlayerID" +
                $",[IsWhile] = @IsWhile" +
                $",[Result] = @Result" +
                $" WHERE ConsignmentPlayerID = {model.ConsignmentPlayerID}", list)) return false;
            if (!PlayerControler.Update(model.player, model.PlayerID)) return false;
            return true;

        }

        public static bool Delete(int id) => StaticResouses.dataBase.ConnChange($"DELETE FROM [dbo].[ConsignmentPlayer] WHERE ConsignmentPlayerID = {id}");

        public static List<ConsignmentPlayer> Get(string str)
        {
            DataSet ds = StaticResouses.dataBase.ConnDataSet(str);
            DataSeter(ds);
            ConsignmentPlayerPlayerGet();
            return models;
        }

        public static List<ConsignmentPlayer> Get()
        {
            reader = StaticResouses.dataBase.Conn("select * from ConsignmentPlayer");
            Reader();
            ConsignmentPlayerPlayerGet();
            return models;
        }

        private static void ConsignmentPlayerPlayerGet()
        {
            foreach (var item in models)
            {
                item.player = PlayerControler.Get().Where(p => p.FIDEID == item.PlayerID).First();
            }
        }

        private static void Reader()
        {
            models = new List<ConsignmentPlayer>();
            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    reader.Close();
                    StaticResouses.dataBase.CloseCon();
                    return;
                }
                models.Add(
                    new ConsignmentPlayer()
                    {
                        ConsignmentPlayerID = Convert.ToInt32(reader["ConsignmentPlayerID"]),
                        ConsignmentID = Convert.ToInt32(reader["ConsignmentID"]),
                        PlayerID = Convert.ToInt32(reader["PlayerID"]),
                        IsWhile = Convert.ToBoolean(reader["IsWhile"]),
                        Result = reader.IsDBNull(reader.GetOrdinal("Result")) ?
                                                                null : Convert.ToDouble(reader["Result"])
                    }
                );
            }
            reader.Close();
            StaticResouses.dataBase.CloseCon();
        }

        private static void DataSeter(DataSet set)
        {
            models = new List<ConsignmentPlayer>();
            foreach (DataRow item in set.Tables[0].Rows)
            {
                models.Add(
                    new ConsignmentPlayer()
                    {
                        ConsignmentPlayerID = Convert.ToInt32(item["ConsignmentPlayerID"]),
                        ConsignmentID = Convert.ToInt32(item["ConsignmentID"]),
                        PlayerID = Convert.ToInt32(item["PlayerID"]),
                        IsWhile = Convert.ToBoolean(item["IsWhile"]),
                        Result = item.IsNull("Result") ? null : Convert.ToDouble(item["Result"])
                    }
                );
            }
        }
    }
}
