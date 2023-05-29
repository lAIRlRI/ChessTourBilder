using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class EventPlayerControler : IEventPlayerControler
    {
        public static EventPlayer nowEventPlayer = new();

        public async Task<bool> Insert(EventPlayer model)
        {
            string messege = await Api.ApiControler.Post($"EventPlayers/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Update(EventPlayer model, int id)
        {
            string messege = await Api.ApiControler.Put($"EventPlayers/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"EventPlayers/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<List<EventPlayer>> GetAll() => JsonConvert.DeserializeObject<List<EventPlayer>>(await Api.ApiControler.Get("EventPlayers/get"));

        public async Task<EventPlayer> GetById(int id) => JsonConvert.DeserializeObject<EventPlayer>(await Api.ApiControler.Get($"EventPlayers/getById?id={id}"));
    }
}