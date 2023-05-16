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
    internal class EventPlayerControler
    {
        public static EventPlayer nowEventPlayer = new();
        static List<EventPlayer> models;

        public static async Task<bool> Insert(EventPlayer model)
        {
            string messege = await Api.ApiControler.Post($"EventPlayers/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Update(EventPlayer model)
        {
            string messege = await Api.ApiControler.Put($"EventPlayers/edit", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"EventPlayers/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<List<EventPlayer>> GetAll()
        {
            models = JsonConvert.DeserializeObject<List<EventPlayer>>(await Api.ApiControler.Get("EventPlayers/get"));
            return models;
        }

        public static async Task<EventPlayer> GetById(int id)
        {
            return JsonConvert.DeserializeObject<EventPlayer>(await Api.ApiControler.Get($"EventPlayers/getById?id={id}"));
        }
    }
}