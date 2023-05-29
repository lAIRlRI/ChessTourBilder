using ChessTourBuilderApp.Data.Model;

namespace ChessTourBuilderApp.Data.Controler
{
    internal interface IEventControler
    {
        Event nowEvent { get; set; }

        Task<bool> Insert(Event model);

        Task<bool> Update(Event model, int id);

        Task<bool> UpdateStatus();

        Task<bool> Delete(int id);

        Task<List<Event>> GetAll();

        Task<List<Event>> GetPublic();

        Task<List<Event>> GetPlayerEvent();

        Task<Event> GetById(int id);

        Task<Event> GetLast();
    }
}