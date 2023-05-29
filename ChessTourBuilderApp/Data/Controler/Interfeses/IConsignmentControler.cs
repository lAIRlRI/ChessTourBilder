using ChessTourBuilderApp.Data.Model;

namespace ChessTourBuilderApp.Data.Controler
{
    internal interface IConsignmentControler
    {
        Consignment nowConsignment { get; set; }

        Task<bool> Insert(Consignment model);

        Task<bool> Update(Consignment model, int id);

        Task<bool> Delete(int id);

        Task<List<Consignment>> GetAll();

        Task<Consignment> GetById(int id);

        Task<List<Consignment>> GetByTourId(int id);

        Task<Consignment> GetLast();
    }
}