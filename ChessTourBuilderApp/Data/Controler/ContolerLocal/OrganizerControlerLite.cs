using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class OrganizerControlerLite : IOrganizerControler
    {
        public Organizer nowOrganizer { get; set; }
        private static List<IDbDataParameter> list;
        private static List<Organizer> models;
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

        public async Task<bool> Insert(Organizer model)
        {
            await Task.Delay(2);
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO Organizer(" +
                                                        "FirstName," +
                                                        "MiddleName," +
                                                        "LastName," +
                                                        "Login," +
                                                        "Password)" +
                                                    "VALUES(" +
                                                        $"@FirstName," +
                                                        $"@MiddleName," +
                                                        $"@LastName," +
                                                        $"@Login," +
                                                        $"@Password)", list.ToArray());
        }

        public async Task<bool> Update(Organizer model, int id)
        {
            await Task.Delay(2);
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE Organizer " +
                $"SET FirstName = @FirstName" +
                $",MiddleName = @MiddleName" +
                $",LastName = @LastName" +
                $",Login = @Login" +
                $",Password = @Password" +
                $" WHERE OrganizerID = {model.OrganizerID}", list.ToArray());
        }

        public async Task<bool> Delete(int id)
        {
            await Task.Delay(2);
            return DataBase.Execute($"DELETE FROM Organizer WHERE OrganizerID = {id}");
        }

        public async Task<bool> GetLogin(string login)
        {
            await Task.Delay(2);
            models = DataBase.Read("select o.*, a.AdministratorID from Organizer o " +
                $"left join Administrator a on o.OrganizerID = a.OrganizerID where Login = '{login}'", mapper);
            return models.Count == 0;
        }

        public async Task<List<Organizer>> GetAll() 
        {
            await Task.Delay(2);
            models = DataBase.Read("select o.*, a.AdministratorID from Organizer o " +
                                                                 "left join Administrator a on o.OrganizerID = a.OrganizerID", mapper);
            return models;
        }

        public async Task<Organizer> GetById(int id) 
        {
            await Task.Delay(2);
            models = DataBase.Read("select o.*, a.AdministratorID from Organizer o " +
                                                                 $"left join Administrator a on o.OrganizerID = a.OrganizerID where OrganizerID = {id}", mapper);
            return models[0];
        }
    }
}