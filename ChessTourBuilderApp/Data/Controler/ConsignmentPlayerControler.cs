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

        public static bool Update(ConsignmentPlayer model)
        {
            SqlParameterSet(model);
            if (!DataBase.Execute($"UPDATE ConsignmentPlayer " +
                $"SET ConsignmentID = @ConsignmentID" +
                $",PlayerID = @PlayerID" +
                $",IsWhile = @IsWhile" +
                $",Result = @Result" +
                $" WHERE ConsignmentPlayerID = {model.ConsignmentPlayerID}", list.ToArray())) return false;
            if (!PlayerControler.Update(model.player, model.PlayerID)) return false;
            return true;

        }

        public static bool Delete(int id) => DataBase.Execute($"DELETE FROM ConsignmentPlayer WHERE ConsignmentPlayerID = {id}");

        public static List<ConsignmentPlayer> Get(string str)
        {
            models = DataBase.Read(str, mapper);
            ConsignmentPlayerPlayerGet();
            return models;
        }

        public static List<ConsignmentPlayer> Get()
        {
            models = DataBase.Read("select * from ConsignmentPlayer", mapper);
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
    }
}