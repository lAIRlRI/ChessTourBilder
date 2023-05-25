using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class EventControler
    {
        public static Event nowEvent = new();

        public static async Task<bool> Insert(Event model)
        {
            string messege = await Api.ApiControler.Post($"Events/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Update(Event model, int id)
        {
            string messege = await Api.ApiControler.Put($"Events/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> UpdateStatus()
        {
            string messege = await Api.ApiControler.Post($"Events/updateStatus");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Events/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<List<Event>> GetAll() => JsonConvert.DeserializeObject<List<Event>>(await Api.ApiControler.Get("Events/get"));

        public static async Task<List<Event>> GetPublic() => JsonConvert.DeserializeObject<List<Event>>(await Api.ApiControler.Get("Events/getPublic"));

        public static async Task<List<Event>> GetPlayerEvent() => JsonConvert.DeserializeObject<List<Event>>(await Api.ApiControler.Get("Events/getPlayerEvent"));

        public static async Task<Event> GetById(int id) => JsonConvert.DeserializeObject<Event>(await Api.ApiControler.Get($"Events/getById?id={id}"));

        public static async Task<Event> GetLast() => JsonConvert.DeserializeObject<Event>(await Api.ApiControler.Get($"Events/getLast"));
    }
}