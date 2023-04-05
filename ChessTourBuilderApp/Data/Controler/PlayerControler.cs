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
    internal class PlayerControler
    {
        public static Player nowPlayer;
        static List<Player> players;
        static SqlDataReader reader;
        static List<SqlParameter> list = new()
                                        {
                                            new SqlParameter() {ParameterName = "@FIDEID" },
                                            new SqlParameter() {ParameterName = "@FirstName" },
                                            new SqlParameter() {ParameterName = "@MiddleName" },
                                            new SqlParameter() {ParameterName = "@LastName" },
                                            new SqlParameter() {ParameterName = "@Birthday" },
                                            new SqlParameter() {ParameterName = "@ELORating" },
                                            new SqlParameter() {ParameterName = "@Contry" }
                                        };


        private static void SqlParameterSet(Player model)
        {
            list[0].Value = model.FIDEID;
            list[1].Value = model.FirstName;
            list[2].Value = model.MiddleName;
            list[3].Value = model.LastName == null ? DBNull.Value : model.LastName;
            list[4].Value = model.Birthday;
            list[5].Value = model.ELORating;
            list[6].Value = model.Contry;
        }

        public static bool Insert(Player model)
        {
            SqlParameterSet(model);
            return StaticResouses.dataBase.ConnChange("INSERT INTO [dbo].[Player](" +
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
                                                                $"@Contry)", list);
        }

        public static bool Update(Player model, int? FIDEID)
        {
            SqlParameterSet(model);
            return StaticResouses.dataBase.ConnChange($"UPDATE [dbo].[Player] " +
                $"SET [FIDEID] = @FIDEID" +
                $",[FirstName] = @FirstName" +
                $",[MiddleName] = @MiddleName" +
                $",[LastName] = @LastName" +
                $",[Birthday] = @Birthday" +
                $",[ELORating] = @ELORating" +
                $",[Contry] = @Contry" +
                $" WHERE FIDEID = {(int)FIDEID}", list);
        }

        public static bool Delete(int? id) => StaticResouses.dataBase.ConnChange($"DELETE FROM [dbo].[Player] WHERE FIDEID = {(int)id}");

        public static List<Player> Get(string str)
        {
            reader = StaticResouses.dataBase.Conn(str);
            Reader();
            return players;
        }

        public static List<Player> Get()
        {
            reader = StaticResouses.dataBase.Conn("SELECT * FROM Player");
            Reader();
            return players;
        }

        private static void Reader()
        {
            players = new List<Player>();
            while (reader.Read())
            {
                players.Add(
                    new Player()
                    {
                        FIDEID = Convert.ToInt32(reader["FIDEID"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Birthday = Convert.ToDateTime(reader["Birthday"]),
                        ELORating = Convert.ToDouble(reader["ELORating"]),
                        Contry = reader["Contry"].ToString()
                    }
                );
            }
            reader.Close();
            StaticResouses.dataBase.CloseCon();
        }

    }
}
