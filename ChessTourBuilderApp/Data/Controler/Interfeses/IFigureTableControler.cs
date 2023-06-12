using ChessTourBuilderApp.Data.ChessClasses;
using ChessTourBuilderApp.Data.Model;

namespace ChessTourBuilderApp.Data.Controler
{
    internal interface IFigureTableControler
    {
        Task<bool> CreateFigureMove(string table);
        Task<bool> InsertFigure(string table, FigureScheme value);
        Task<List<FigureScheme>> GetAll(string table);
        Task<bool> UpdatePozition(string table, bool IsMoving, UpdateFigureModel view);
        Task<bool> UpdateInGame(string table, UpdateFigureModel view);
        Task<bool> UpdateEat(string table, UpdateFigureModel view);
        Task<bool> UpdateCastlingLong(string table, UpdateFigureModel view);
        Task<bool> UpdateCastlingShort(string table, UpdateFigureModel view);
    }
}