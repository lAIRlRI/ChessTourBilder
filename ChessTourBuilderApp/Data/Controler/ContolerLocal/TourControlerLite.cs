using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class TourControlerLite : ITourControler
    {
        public Tour nowTour { get; set; }

        static List<Tour> models;
        private static List<IDbDataParameter> list;
        private static readonly Func<IDataReader, Tour> mapper = r => new Tour()
        {
            TourID = Convert.ToInt32(r["TourID"]),
            NameTour = r["NameTour"].ToString(),
            EventID = Convert.ToInt32(r["EventID"])
        };


        private static void SqlParameterSet(Tour model)
        {
            list = DataBase.SetParameters
                (
                    new List<ParametrBD>()
                    {
                        new ParametrBD("@EventID",model.EventID),
                        new ParametrBD("@NameTour",model.NameTour)
                    }
                );
        }

        public async Task<bool> Insert(Tour model)
        {
            await Task.Delay(2);
            SqlParameterSet(model);
            return DataBase.Execute("INSERT INTO Tour(EventID,NameTour)" +
                                                          "VALUES(@EventID,@NameTour)", list.ToArray());
        }

        public async Task<bool> Update(Tour model, int id)
        {
            await Task.Delay(2);
            SqlParameterSet(model);
            return DataBase.Execute($"UPDATE Tour " +
                $"SET EventID = @EventID" +
                $",NameTour = @NameTour" +
                $" WHERE ID = {model.TourID}", list.ToArray());
        }

        public async Task<bool> Delete(int id)
        {
            await Task.Delay(2);
            return DataBase.Execute($"DELETE FROM Tour WHERE TourID = {id}");
        }

        public async Task<List<Tour>> GetAll() 
        {
            await Task.Delay(2);
            models = DataBase.Read("SELECT * FROM Tour", mapper);
            return models;
        }

        public async Task<List<Tour>> GetByEventId(int id)
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM Tour WHERE EventId = {id}", mapper);
            return models;
        }

        public async Task<Tour> GetById(int id) 
        {
            await Task.Delay(2);
            models = DataBase.Read($"SELECT * FROM Tour WHERE TourID = {id}", mapper);
            return models[0];
        }

        public async Task<Tour> GetLast()
        {
            await Task.Delay(2);
            models = DataBase.Read("SELECT * FROM Tour where TourID = (select max(TourID) from Tour)", mapper);
            return models[0];
        }
    }
}