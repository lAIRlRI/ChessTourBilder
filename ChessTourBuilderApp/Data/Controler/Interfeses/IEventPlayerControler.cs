using ChessTourBuilderApp.Data.Model;

namespace ChessTourBuilderApp.Data.Controler
{
    internal interface IEventPlayerControler
    {
        public static EventPlayer nowEventPlayer = new();
        
        Task<bool> Insert(EventPlayer model);
        
        Task<bool> Update(EventPlayer model, int id);
        
        Task<bool> Delete(int id);
        
        Task<List<EventPlayer>> GetAll();
        
        Task<EventPlayer> GetById(int id);
    }
}