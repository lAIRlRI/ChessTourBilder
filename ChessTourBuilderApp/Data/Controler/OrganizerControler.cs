using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class OrganizerControler
    {
        public static Organizer nowOrganizer;
        static List<Organizer> models;
        static SqlDataReader reader;
        static List<SqlParameter> list = new()
                                        {
                                            new SqlParameter() {ParameterName = "@FirstName" },
                                            new SqlParameter() {ParameterName = "@MiddleName" },
                                            new SqlParameter() {ParameterName = "@LastName" },
                                            new SqlParameter() {ParameterName = "@Login" },
                                            new SqlParameter() {ParameterName = "@Password" }
                                        };


        private static void SqlParameterSet(Organizer model)
        {
            list[0].Value = model.FirstName;
            list[1].Value = model.MiddleName;
            list[2].Value = model.LastName == null ? DBNull.Value : model.LastName;
            list[3].Value = model.Login;
            list[4].Value = model.Password;
        }

        public static bool Insert(Organizer model)
        {
            SqlParameterSet(model);
            return DataBase.ConnChange("INSERT INTO [dbo].[Organizer](" +
                                                                "[FirstName]," +
                                                                "[MiddleName]," +
                                                                "[LastName]," +
                                                                "[Login]," +
                                                                "[Password])" +
                                                          "VALUES(" +
                                                                $"@FirstName," +
                                                                $"@MiddleName," +
                                                                $"@LastName," +
                                                                $"@Login," +
                                                                $"@Password)", list);
        }

        public static bool Update(Organizer model)
        {
            SqlParameterSet(model);
            return DataBase.ConnChange($"UPDATE [dbo].[Organizer] " +
                $"SET [FirstName] = @FirstName" +
                $",[MiddleName] = @MiddleName" +
                $",[LastName] = @LastName" +
                $",[Login] = @Login" +
                $",[Password] = @Password" +
                $" WHERE OrganizerID = {model.OrganizerID}", list);
        }

        public static bool Delete(int id) => DataBase.ConnChange($"DELETE FROM [dbo].[Organizer] WHERE OrganizerID = {id}");

        public static List<Organizer> Get(string str)
        {
            reader = DataBase.Conn(str);
            Reader();
            return models;
        }

        public static List<Organizer> Get()
        {
            reader = DataBase.Conn("select o.*, a.AdministratorID from Organizer o " +
                                                                  "left join Administrator a on o.OrganizerID = a.OrganizerID");
            Reader();
            return models;
        }

        private static void Reader()
        {
            models = new List<Organizer>();
            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    reader.Close();
                    DataBase.CloseCon();
                    return;
                }
                models.Add(
                    new Organizer()
                    {
                        OrganizerID = Convert.ToInt32(reader["OrganizerID"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Login = reader["Login"].ToString(),
                        Password = reader["Password"].ToString(),
                        Administrator = reader.IsDBNull(reader.GetOrdinal("AdministratorID")) ?
                                                                -1 : Convert.ToInt32(reader["AdministratorID"])

                    }
                );
            }
            reader.Close();
            DataBase.CloseCon();
        }
    }
}
