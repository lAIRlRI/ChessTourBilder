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
    internal class PlayerControler
    {
        public static Player nowPlayer;
        static List<Player> models;

        public static async Task<bool> Insert(Player model)
        {
            string messege = await Api.ApiControler.Post($"Players/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Update(Player model)
        {
            string messege = await Api.ApiControler.Put($"Players/edit", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Players/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<List<Player>> GetAll()
        {
            models = JsonConvert.DeserializeObject<List<Player>>(await Api.ApiControler.Get("Players/get"));
            return models;
        }

        public static async Task<Player> GetById(int id)
        {
            return JsonConvert.DeserializeObject<Player>(await Api.ApiControler.Get($"Players/getById?id={id}"));
        }

        public static async Task<List<Player>> GetByEventId(int id)
        {
            return JsonConvert.DeserializeObject<List<Player>>(await Api.ApiControler.Get($"Players/getByEventId?id={id}"));
        }

        public static async Task<bool> GetLogin(string login)
        {
            string messege = await Api.ApiControler.Get($"Players/getLogin?login={login}");
            if (messege == "Nice") return true;
            return false;
        }
    }
}