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
    internal class OrganizerControler
    {
        public static Organizer nowOrganizer;
        private static List<Organizer> models;
        private static List<IDbDataParameter> list;
        private static readonly Func<IDataReader, Organizer> mapper = r => new Organizer()
        {
            OrganizerID = Convert.ToInt32(r["OrganizerID"]),
            FirstName = r["FirstName"].ToString(),
            MiddleName = r["MiddleName"].ToString(),
            LastName = r["LastName"].ToString(),
            Login = r["Login"].ToString(),
            Password = r["Password"].ToString(),
            Administrator = r.IsDBNull(r.GetOrdinal("AdministratorID")) ? -1 : Convert.ToInt32(r["AdministratorID"])
        };

        private static void SqlParameterSet(Organizer model)
        {

            list = DataBase.SetParameters
                 (
                     new List<ParametrBD>()
                     {
                        new ParametrBD("@FirstName", model.FirstName),
                        new ParametrBD("@MiddleName", model.MiddleName),
                        new ParametrBD("@LastName",model.LastName == null ? DBNull.Value : model.LastName),
                        new ParametrBD("@Login",model.Login),
                        new ParametrBD("@Password",model.Password)
                     }
                 );
        }

        public static bool Insert(Organizer model)
        {
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO [dbo].[Organizer](" +
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
                                                                $"@Password)", list.ToArray());
        }

        public static bool Update(Organizer model)
        {
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE [dbo].[Organizer] " +
                $"SET [FirstName] = @FirstName" +
                $",[MiddleName] = @MiddleName" +
                $",[LastName] = @LastName" +
                $",[Login] = @Login" +
                $",[Password] = @Password" +
                $" WHERE OrganizerID = {model.OrganizerID}", list.ToArray());
        }

        public static bool Delete(int id) => DataBase.Execute($"DELETE FROM [dbo].[Organizer] WHERE OrganizerID = {id}");

        public static List<Organizer> Get(string str)
        {
            models = DataBase.Read(str, mapper);
            return models;
        }

        public static List<Organizer> Get()
        {
            models = DataBase.Read("select o.*, a.AdministratorID from Organizer o " +
                                                                  "left join Administrator a on o.OrganizerID = a.OrganizerID", mapper);
            return models;
        }
    }
}