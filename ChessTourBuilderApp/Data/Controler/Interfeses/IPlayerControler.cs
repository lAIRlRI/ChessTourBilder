using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler
{
    internal interface IPlayerControler
    {
        Player nowPlayer { get; set; }

        Task<bool> Insert(Player model);
        Task<bool> Update(Player model, int id);
        Task<bool> Delete(int id);
        Task<List<Player>> GetAll();
        Task<Player> GetById(int id);
        Task<List<Player>> GetByEventId(int id);
        Task<bool> GetLogin(string login);
    }
}