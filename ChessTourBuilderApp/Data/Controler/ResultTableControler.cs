using ChessTourBuilderApp.Data.ChessClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class ResultTableControler
    {
        public static async Task<bool> CreateResultTable(string table)
        {
            string messege = await Api.ApiControler.Get($"FigureTableControler/createResultTable?table={table}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> InsertResult(string table, FigureScheme value)
        {
            string messege = await Api.ApiControler.Post($"FigureTableControler/insertResult?table={table}", value);
            if (messege == "-1") return true;
            return false;
        }


    }
}
