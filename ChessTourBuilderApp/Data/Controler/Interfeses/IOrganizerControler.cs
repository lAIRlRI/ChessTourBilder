using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler
{
    internal interface IOrganizerControler
    {
        Organizer nowOrganizer { get; set; }

        Task<bool> Insert(Organizer model);
        Task<bool> Update(Organizer model, int id);
        Task<bool> Delete(int id);
        Task<bool> GetLogin(string login);
        Task<List<Organizer>> GetAll();
        Task<Organizer> GetById(int id);
    }
}