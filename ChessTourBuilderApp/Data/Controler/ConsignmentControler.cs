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
    internal class ConsignmentControler
    {
        public static Consignment nowConsignment;
        static List<Consignment> models;

        public static async Task<bool> Insert(Consignment model)
        {
            string messege = await Api.ApiControler.Post($"Consignments/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Update(Consignment model, int id)
        {
            string messege = await Api.ApiControler.Put($"Consignments/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Consignments/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<List<Consignment>> GetAll()
        {
            models = JsonConvert.DeserializeObject<List<Consignment>>(await Api.ApiControler.Get("Consignments/get"));
            return models;
        }

        public static async Task<Consignment> GetById(int id) => JsonConvert.DeserializeObject<Consignment>(await Api.ApiControler.Get($"Consignments/getById?id={id}"));

        public static async Task<List<Consignment>> GetByTourId(int id) => JsonConvert.DeserializeObject<List<Consignment>>(await Api.ApiControler.Get($"Consignments/getByTourId?id={id}"));

        public static async Task<Consignment> GetLast() => JsonConvert.DeserializeObject<Consignment>(await Api.ApiControler.Get($"Consignments/getLast"));
    }
}