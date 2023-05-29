using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class ResultTableControler : IResultTableControler
    {
        public async Task<bool> CreateResultTable(string table)
        {
            string messege = await Api.ApiControler.Get($"TableResultController/createResultTable?table={table}");
            if (messege == "Nice") return true;
            return false;
        }

        public async Task<bool> InsertResult(string table, TableResult value)
        {
            string messege = await Api.ApiControler.Post($"TableResultController/insertResult?table={table}", value);
            if (messege == "-1") return true;
            return false;
        }

        public async Task<List<ResultSheme>> GetResultTable(string table) => JsonConvert.DeserializeObject<List<ResultSheme>>(await Api.ApiControler.Get($"TableResultController/getResultTable?table={table}"));

        public async Task<List<ResultSheme>> GetResultTableСircle(string table) => JsonConvert.DeserializeObject<List<ResultSheme>>(await Api.ApiControler.Get($"TableResultController/getResultTableСircle?table={table}"));
    }
}
