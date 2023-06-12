using ChessTourBuilderApp.Data.Controler.ContolerLocal;
using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class ConsignmentControlerLite : IConsignmentControler
    {
        public Consignment nowConsignment { get; set; }
        static List<Consignment> models;
        static List<IDbDataParameter> list;

        static Func<IDataReader, Consignment> mapper = r => new Consignment()
        {
            ConsignmentID = Convert.ToInt32(r["ConsignmentID"]),
            TourID = Convert.ToInt32(r["TourID"]),
            StatusID = Convert.ToInt32(r["StatusID"]),
            DateStart = Convert.ToDateTime(r["DateStart"]),
            GameMove = r["GameMove"].ToString()
        };

        private static void SqlParameterSet(Consignment model)
        {
            list = DataBase.SetParameters
                (
                    new List<ParametrBD>()
                    {
                        new ParametrBD("@TourID", model.TourID),
                        new ParametrBD("@StatusID", model.StatusID),
                        new ParametrBD("@DateStart", model.DateStart),
                        new ParametrBD("@GameMove", model.GameMove == null ? DBNull.Value : model.GameMove)
                    }
                );
        }

        public async Task<bool> Insert(Consignment model)
        {
            SqlParameterSet(model);
            await Task.Delay(2);
            if (!DataBase.Execute("INSERT INTO Consignment(" +
                                                        "TourID," +
                                                        "StatusID," +
                                                        "DateStart)" +
                                                    "VALUES(" +
                                                        $"@TourID," +
                                                        $"@StatusID," +
                                                        $"@DateStart)", list.ToArray())) return false;

            int modelID = (await GetLast()).ConsignmentID;
            model.whitePlayer.ConsignmentID = modelID;
            model.whitePlayer.IsWhile = true;

            model.blackPlayer.ConsignmentID = modelID;
            model.blackPlayer.IsWhile = false;
            
            if (!ConsignmentPlayerControlerLite.Insert(model.whitePlayer)) return false;
            if (!ConsignmentPlayerControlerLite.Insert(model.blackPlayer)) return false;

            return true;
        }

        public async Task<bool> Update(Consignment model, int id)
        {
            SqlParameterSet(model);
            await Task.Delay(2);
            if (!DataBase.Execute($"UPDATE Consignment " +
                $"SET TourID = @TourID" +
                $",StatusID = @StatusID" +
                $",DateStart = @DateStart" +
                $",GameMove = @GameMove" +
                $" WHERE ConsignmentID = {model.ConsignmentID}", list.ToArray())) return false;

            if (!await ConsignmentPlayerControlerLite.Update(model.blackPlayer)) return false;
            if (!await ConsignmentPlayerControlerLite.Update(model.whitePlayer)) return false;

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            await Task.Delay(2);
            return DataBase.Execute($"DELETE FROM Consignment WHERE ConsignmentID = {id}");
        }

        public async Task<List<Consignment>> GetAll()
        {
            await Task.Delay(2);
            models = DataBase.Read("SELECT * FROM Consignment", mapper);
            await ConsignmentPlayerGet();
            return models;
        }

        public async Task<Consignment> GetById(int id)
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM Consignment where ConsignmentID = {id}", mapper);
            await ConsignmentPlayerGet();
            return models[0];
        }

        public async Task<List<Consignment>> GetByTourId(int id)
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM Consignment where TourID = {id}", mapper);
            await ConsignmentPlayerGet();
            return models;
        }

        public async Task<Consignment> GetLast()
        {
            await Task.Delay(2);
            models = DataBase.Read("SELECT * FROM Consignment where ConsignmentID = (select max(ConsignmentID) from Consignment)", mapper);
            await ConsignmentPlayerGet();
            return models[0];
        }

        private async static Task ConsignmentPlayerGet()
        {
            foreach (var item in models)
            {
                List<ConsignmentPlayer> consignmentPlayers = await ConsignmentPlayerControlerLite.Get($"select * from ConsignmentPlayer where ConsignmentID = {item.ConsignmentID}");
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
    }
}