using ChessTourBuilderApp.Data.Model;


namespace ChessTourBuilderApp.Data.Controler
{
    internal interface ITourControler
    {
        Tour nowTour { get; set; }

        Task<bool> Insert(Tour model);
        Task<bool> Update(Tour model, int id);
        Task<bool> Delete(int id);
        Task<List<Tour>> GetAll();
        Task<List<Tour>> GetByEventId(int id);
        Task<Tour> GetById(int id);
        Task<Tour> GetLast();
    }
}