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
    internal class TourControler
    {
        public static Tour nowTour = new();

        public static async Task<bool> Insert(Tour model)
        {
            string messege = await Api.ApiControler.Post($"Tours/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Update(Tour model, int id)
        {
            string messege = await Api.ApiControler.Put($"Tours/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Tours/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<List<Tour>> GetAll() => JsonConvert.DeserializeObject<List<Tour>>(await Api.ApiControler.Get("Tours/get"));

        public static async Task<List<Tour>> GetByEventId(int id) => JsonConvert.DeserializeObject<List<Tour>>(await Api.ApiControler.Get($"Tours/getByEventId?id={id}"));

        public static async Task<Tour> GetById(int id) => JsonConvert.DeserializeObject<Tour>(await Api.ApiControler.Get($"Tours/getById?id={id}"));

        public static async Task<Tour> GetLast() => JsonConvert.DeserializeObject<Tour>(await Api.ApiControler.Get($"Tours/getLast"));
    }
}