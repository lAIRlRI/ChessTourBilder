using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler
{
    internal interface IResultTableControler
    {
        Task<bool> CreateResultTable(string table);
        Task<bool> InsertResult(string table, TableResult value);
        Task<List<ResultSheme>> GetResultTable(string table);
        Task<List<ResultSheme>> GetResultTableСircle(string table);
    }
}
