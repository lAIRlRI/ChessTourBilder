using ChessTourBuilderApp.Data.ChessClasses;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class FigureTableControler
    {
        public static async Task<bool> CreateFigureMove( string table)
        {
            string messege = await Api.ApiControler.Get($"FigureTableControler/createFigureMove?table={table}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> InsertFigure( string table, FigureScheme value)
        {
            string messege = await Api.ApiControler.Post($"FigureTableControler/InsertFigure?table={table}", value);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<List<FigureScheme>> GetAll(string table)
        {

            string input = await Api.ApiControler.Get($"FigureTableControler/getAll?table={table}");

            return JsonConvert.DeserializeObject<List<FigureScheme>>(input,
                   new JsonSerializerSettings
                   {
                       NullValueHandling = NullValueHandling.Ignore
                   });
        }

        public static async Task<bool> UpdatePozition(string table, bool IsMoving, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updatePozition?table={table}&IsMoving={IsMoving}", view);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> UpdateInGame(string table, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updateInGame?table={table}", view);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> UpdateEat(string table, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updateEat?table={table}", view);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> UpdateCastlingLong(string table, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updateCastlingLong?table={table}", view);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> UpdateCastlingShort(string table, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updateCastlingShort?table={table}", view);
            if (messege == "Nice") return true;
            return false;
        }
    }
}