using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class PlayerControlerLite : IPlayerControler
    {
        public Player nowPlayer { get; set; } = new Player();
        static List<Player> models;
        private static List<IDbDataParameter> list;
        public static readonly Func<IDataReader, Player> mapper = r => new Player()
        {
            FIDEID = Convert.ToInt32(r["FIDEID"]),
            FirstName = r["FirstName"].ToString(),
            MiddleName = r["MiddleName"].ToString(),
            LastName = r["LastName"].ToString(),
            Passord = r["Passord"].ToString(),
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
                        new ParametrBD("@Passord",model.Passord),
                        new ParametrBD("@LastName",model.LastName == null ? DBNull.Value : model.LastName),
                        new ParametrBD("@Birthday" ,model.Birthday),
                        new ParametrBD("@ELORating",model.ELORating),
                        new ParametrBD("@Contry",model.Contry)
                     }
                 );
        }

        public async Task<bool> Insert(Player model)
        {
            await Task.Delay(2);
            model.Passord = Helper.GeneratePassword(8);
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO Player(" +
                                                    "FIDEID," +
                                                    "FirstName," +
                                                    "MiddleName," +
                                                    "LastName," +
                                                    "Passord," +
                                                    "Birthday," +
                                                    "ELORating," +
                                                    "Contry)" +
                                                "VALUES(" +
                                                    $"@FIDEID," +
                                                    $"@FirstName," +
                                                    $"@MiddleName," +
                                                    $"@LastName," +
                                                    $"@Passord," +
                                                    $"@Birthday," +
                                                    $"@ELORating," +
                                                    $"@Contry)", list.ToArray());
        }

        public async Task<bool> Update(Player model, int id)
        {
            await Task.Delay(2);
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE Player " +
                $"SET FIDEID = @FIDEID" +
                $",FirstName = @FirstName" +
                $",MiddleName = @MiddleName" +
                $",LastName = @LastName" +
                $",Passord = @Passord" +
                $",Birthday = @Birthday" +
                $",ELORating = @ELORating" +
                $",Contry = @Contry" +
                $" WHERE FIDEID = {id}", list.ToArray());
        }

        public async Task<bool> Delete(int id)
        {
            await Task.Delay(2);
            return DataBase.Execute($"DELETE FROM Player WHERE FIDEID = {id}");
        }

        public async Task<List<Player>> GetAll() 
        {
            await Task.Delay(2);
            models = DataBase.Read("SELECT * FROM Player", mapper);
            return models;
        }

        public async Task<Player> GetById(int id) 
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM Player WHERE FIDEID = {id}", mapper);
            return models[0];
        }

        public async Task<List<Player>> GetByEventId(int id) 
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT p.* FROM EventPlayer ep JOIN Player p ON ep.PlayerID = p.FIDEID WHERE ep.EventID = {id};", mapper);
            return models;
        }

        public async Task<bool> GetLogin(string login)
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM Player WHERE FIDEID = {login};", mapper);
            return models.Count == 0;
        }

        public async Task<bool> InsertAdmin()
        {
            await Task.Delay(2);
            return DataBase.Execute($"INSERT INTO Administrator ([OrganizerID]) VALUES (1)");
        }
    }
}