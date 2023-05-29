using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class ConsignmentControler : IConsignmentControler
    {
        public Consignment nowConsignment { get; set; }
        static List<Consignment> models;

        public async Task<bool> Insert(Consignment model)
        {
            string messege = await Api.ApiControler.Post($"Consignments/create", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Update(Consignment model, int id)
        {
            string messege = await Api.ApiControler.Put($"Consignments/edit?id={id}", model);
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            string messege = await Api.ApiControler.Delete($"Consignments/delete?id={id}");
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<List<Consignment>> GetAll()
        {
            models = JsonConvert.DeserializeObject<List<Consignment>>(await Api.ApiControler.Get("Consignments/get"));
            return models;
        }

        public async Task<Consignment> GetById(int id) => JsonConvert.DeserializeObject<Consignment>(await Api.ApiControler.Get($"Consignments/getById?id={id}"));

        public async Task<List<Consignment>> GetByTourId(int id) => JsonConvert.DeserializeObject<List<Consignment>>(await Api.ApiControler.Get($"Consignments/getByTourId?id={id}"));

        public async Task<Consignment> GetLast() => JsonConvert.DeserializeObject<Consignment>(await Api.ApiControler.Get($"Consignments/getLast"));
    }
}