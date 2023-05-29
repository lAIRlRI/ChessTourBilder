using ChessTourBuilderApp.Data.ChessClasses;
using ChessTourBuilderApp.Data.Model;

namespace ChessTourBuilderApp.Data.Controler
{
    internal interface IMoveTableControler
    {
        Task<bool> CreateTableMove(string table);
        Task<bool> PutWinner(string table, int ID);
        Task<bool> PutLastMove(string table);
        Task<bool> PostMove(string table, MoveTableModel value);
        Task<bool> DeleteLastMove(string table);
        Task<MovePozition> GetMovePozition(string table);
    }
}