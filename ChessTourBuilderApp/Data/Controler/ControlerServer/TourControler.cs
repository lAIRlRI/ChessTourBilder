﻿using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class TourControler : ITourControler
    {
        public Tour nowTour { get; set; }

        public async Task<bool> Insert(Tour model)
        {
            string messege = await Api.ApiControler.Post($"Tours/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Update(Tour model, int id)
        {
            string messege = await Api.ApiControler.Put($"Tours/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Tours/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<List<Tour>> GetAll() => JsonConvert.DeserializeObject<List<Tour>>(await Api.ApiControler.Get("Tours/get"));

        public async Task<List<Tour>> GetByEventId(int id) => JsonConvert.DeserializeObject<List<Tour>>(await Api.ApiControler.Get($"Tours/getByEventId?id={id}"));

        public async Task<Tour> GetById(int id) => JsonConvert.DeserializeObject<Tour>(await Api.ApiControler.Get($"Tours/getById?id={id}"));

        public async Task<Tour> GetLast() => JsonConvert.DeserializeObject<Tour>(await Api.ApiControler.Get($"Tours/getLast"));
    }
}