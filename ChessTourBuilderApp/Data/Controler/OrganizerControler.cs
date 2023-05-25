using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class OrganizerControler
    {
        public static Organizer nowOrganizer;

        public static async Task<bool> Insert(Organizer model)
        {
            string messege = await Api.ApiControler.Post($"Organizers/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Update(Organizer model, int id)
        {
            string messege = await Api.ApiControler.Put($"Organizers/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Organizers/delete?id={id}");
            if (messege == "Nice") return true; 
            return false;
        }

        public static async Task<bool> GetLogin(string login)
        {
            string messege = await Api.ApiControler.Get($"Organizers/getLogin?login={login}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<List<Organizer>> GetAll() => JsonConvert.DeserializeObject<List<Organizer>>(await Api.ApiControler.Get("Organizers/get"));

        public static async Task<Organizer> GetById(int id) => JsonConvert.DeserializeObject<Organizer>(await Api.ApiControler.Get($"Organizers/getById?id={id}"));
    }
}