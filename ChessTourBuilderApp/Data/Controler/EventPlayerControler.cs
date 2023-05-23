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

        public static async Task<bool> Insert(EventPlayer model)
        {
            string messege = await Api.ApiControler.Post($"EventPlayers/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Update(EventPlayer model, int id)
        {
            string messege = await Api.ApiControler.Put($"EventPlayers/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"EventPlayers/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<List<EventPlayer>> GetAll() => JsonConvert.DeserializeObject<List<EventPlayer>>(await Api.ApiControler.Get("EventPlayers/get"));

        public static async Task<EventPlayer> GetById(int id) => JsonConvert.DeserializeObject<EventPlayer>(await Api.ApiControler.Get($"EventPlayers/getById?id={id}"));
    }
}