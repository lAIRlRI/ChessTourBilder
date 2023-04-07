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
    internal class PlayerControler
    {
        public static Player nowPlayer;
        static List<Player> models;
        private static List<IDbDataParameter> list;
        public static readonly Func<IDataReader, Player> mapper = r => new Player()
        {
            FIDEID = Convert.ToInt32(r["FIDEID"]),
            FirstName = r["FirstName"].ToString(),
            MiddleName = r["MiddleName"].ToString(),
            LastName = r["LastName"].ToString(),
            Birthday = Convert.ToDateTime(r["Birthday"]),
            ELORating = Convert.ToDouble(r["ELORating"]),
            Contry = r["Contry"].ToString()
        };

        private static void SqlParameterSet(Player model)
        {

            list = DataBase.SetParameters
                 (
                     new List<ParametrBD>()
                     {
                        new ParametrBD("@FIDEID" ,model.FIDEID),
                        new ParametrBD("@FirstName" ,model.FirstName),
                        new ParametrBD("@MiddleName",model.MiddleName),
                        new ParametrBD("@LastName",model.LastName == null ? DBNull.Value : model.LastName),
                        new ParametrBD("@Birthday" ,model.Birthday),
                        new ParametrBD("@ELORating",model.ELORating),
                        new ParametrBD("@Contry",model.Contry)
                     }
                 );
        }

        public static bool Insert(Player model)
        {
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO [dbo].[Player](" +
                                                                "[FIDEID]," +
                                                                "[FirstName]," +
                                                                "[MiddleName]," +
                                                                "[LastName]," +
                                                                "[Birthday]," +
                                                                "[ELORating]," +
                                                                "[Contry])" +
                                                          "VALUES(" +
                                                                $"@FIDEID," +
                                                                $"@FirstName," +
                                                                $"@MiddleName," +
                                                                $"@LastName," +
                                                                $"@Birthday," +
                                                                $"@ELORating," +
                                                                $"@Contry)", list.ToArray());
        }

        public static bool Update(Player model, int? FIDEID)
        {
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE [dbo].[Player] " +
                $"SET [FIDEID] = @FIDEID" +
                $",[FirstName] = @FirstName" +
                $",[MiddleName] = @MiddleName" +
                $",[LastName] = @LastName" +
                $",[Birthday] = @Birthday" +
                $",[ELORating] = @ELORating" +
                $",[Contry] = @Contry" +
                $" WHERE FIDEID = {(int)FIDEID}", list.ToArray());
        }

        public static bool Delete(int? id) => DataBase.Execute($"DELETE FROM [dbo].[Player] WHERE FIDEID = {(int)id}");

        public static List<Player> Get(string str)
        {
            models = DataBase.Read(str, mapper);
            return models;
        }

        public static List<Player> Get()
        {
            models = DataBase.Read("SELECT * FROM Player", mapper);
            return models;
        }
    }
}