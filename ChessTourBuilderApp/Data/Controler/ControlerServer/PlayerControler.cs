using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class PlayerControler : IPlayerControler
    {
        public Player nowPlayer { get; set; }

        public async Task<bool> Insert(Player model)
        {
            model.Passord = Helper.GeneratePassword(8);
            string messege = await Api.ApiControler.Post($"Players/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Update(Player model, int id)
        {
            string messege = await Api.ApiControler.Put($"Players/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Players/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<List<Player>> GetAll() => JsonConvert.DeserializeObject<List<Player>>(await Api.ApiControler.Get("Players/get"));

        public async Task<Player> GetById(int id) => JsonConvert.DeserializeObject<Player>(await Api.ApiControler.Get($"Players/getById?id={id}"));

        public async Task<List<Player>> GetByEventId(int id) => JsonConvert.DeserializeObject<List<Player>>(await Api.ApiControler.Get($"Players/getByEventId?id={id}"));

        public async Task<bool> GetLogin(string login)
        {
            string messege = await Api.ApiControler.Get($"Players/getLogin?login={login}");
            if (messege == "Nice") return true;
            return false;
        }
    }
}