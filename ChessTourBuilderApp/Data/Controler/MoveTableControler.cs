using ChessTourBuilderApp.Data.ChessClasses;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class MoveTableControler
    {
        public static async Task<bool> CreateTableMove(string table)
        {
            string messege = await Api.ApiControler.Get($"MoveTableControler/createTableMove?table={table}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> PutWinner(string table, int ID)
        {
            string messege = await Api.ApiControler.Put($"MoveTableControler/putWinner?table={table}&ID={ID}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> PutLastMove(string table)
        {
            string messege = await Api.ApiControler.Put($"MoveTableControler/putLastMove?table={table}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> PostMove( string table, MoveTableModel value)
        {
            string messege = await Api.ApiControler.Post($"MoveTableControler/postMove?table={table}", value);
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> DeleteLastMove(string table)
        {
            string messege = await Api.ApiControler.Delete($"MoveTableControler/deleteLastMove?table={table}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<MovePozition> GetMovePozition(string table) => JsonConvert.DeserializeObject<MovePozition>(await Api.ApiControler.Get($"MoveTableControler/getMovePozition?table={table}"));
    }
}