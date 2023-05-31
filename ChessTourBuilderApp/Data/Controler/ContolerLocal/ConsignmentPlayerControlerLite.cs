﻿using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ContolerLocal
{
    internal class ConsignmentPlayerControlerLite
    {
        static List<ConsignmentPlayer> models;
        static List<IDbDataParameter> list;

        static Func<IDataReader, ConsignmentPlayer> mapper = r => new ConsignmentPlayer()
        {
            ConsignmentPlayerID = Convert.ToInt32(r["ConsignmentPlayerID"]),
            ConsignmentID = Convert.ToInt32(r["ConsignmentID"]),
            PlayerID = Convert.ToInt32(r["PlayerID"]),
            IsWhile = Convert.ToBoolean(r["IsWhile"]),
            Result = r.IsDBNull(r.GetOrdinal("Result")) ? null : Convert.ToDouble(r["Result"])
        };


        private static void SqlParameterSet(ConsignmentPlayer model)
        {
            list = DataBase.SetParameters
                 (
                     new List<ParametrBD>()
                     {
                        new ParametrBD("@ConsignmentID",model.ConsignmentID),
                        new ParametrBD("@PlayerID",model.PlayerID),
                        new ParametrBD("@IsWhile",model.IsWhile),
                        new ParametrBD("@Result",model.Result == null ? DBNull.Value : model.Result )
                     }
                 );
        }

        public static bool Insert(ConsignmentPlayer model)
        {
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO ConsignmentPlayer(" +
                                                                "ConsignmentID," +
                                                                "PlayerID," +
                                                                "IsWhile," +
                                                                "Result)" +
                                                          "VALUES(" +
                                                                $"@ConsignmentID," +
                                                                $"@PlayerID," +
                                                                $"@IsWhile," +
                                                                $"@Result)", list.ToArray());
        }

        public static async Task<bool> Update(ConsignmentPlayer model)
        {
            SqlParameterSet(model);
            if (!DataBase.Execute($"UPDATE ConsignmentPlayer " +
                $"SET ConsignmentID = @ConsignmentID" +
                $",PlayerID = @PlayerID" +
                $",IsWhile = @IsWhile" +
                $",Result = @Result" +
                $" WHERE ConsignmentPlayerID = {model.ConsignmentPlayerID}", list.ToArray())) return false;
            if (!await StaticResouses.mainControler.PlayerControler.Update(model.player, model.PlayerID)) return false;
            return true;
        }

        public static bool Delete(int id) => DataBase.Execute($"DELETE FROM ConsignmentPlayer WHERE ConsignmentPlayerID = {id}");

        public static async Task<List<ConsignmentPlayer>> Get()
        {
            models = DataBase.Read("select * from ConsignmentPlayer", mapper);
            await ConsignmentPlayerPlayerGet();
            return models;
        }

        public static async Task<List<ConsignmentPlayer>> Get(string str)
        {
            models = DataBase.Read(str, mapper);
            await ConsignmentPlayerPlayerGet();
            return models;
        }

        private static async Task ConsignmentPlayerPlayerGet()
        {
            foreach (var item in models)
            {
                item.player = await StaticResouses.mainControler.PlayerControler.GetById(item.PlayerID);
            }
        }
    }
}
