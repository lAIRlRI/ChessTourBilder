using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class EventControler : IEventControler
    {
        public Event nowEvent { get; set; } = new();

        public async Task<bool> Insert(Event model)
        {
            string messege = await Api.ApiControler.Post($"Events/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Update(Event model, int id)
        {
            string messege = await Api.ApiControler.Put($"Events/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> UpdateStatus()
        {
            string messege = await Api.ApiControler.Post($"Events/updateStatus");
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Events/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<List<Event>> GetAll() => JsonConvert.DeserializeObject<List<Event>>(await Api.ApiControler.Get("Events/get"));

        public async Task<List<Event>> GetPublic() => JsonConvert.DeserializeObject<List<Event>>(await Api.ApiControler.Get("Events/getPublic"));

        public async Task<List<Event>> GetPlayerEvent() => JsonConvert.DeserializeObject<List<Event>>(await Api.ApiControler.Get("Events/getPlayerEvent"));

        public async Task<Event> GetById(int id) => JsonConvert.DeserializeObject<Event>(await Api.ApiControler.Get($"Events/getById?id={id}"));

        public async Task<Event> GetLast() => JsonConvert.DeserializeObject<Event>(await Api.ApiControler.Get($"Events/getLast"));
    }
}