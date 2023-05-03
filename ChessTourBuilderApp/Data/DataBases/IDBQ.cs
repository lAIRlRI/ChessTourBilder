using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.DataBases
{
    internal interface IDBQ
    {
        string GetTableMove(string table);
        string GetTableFigures(string table);
        string GetWinner(string table, int ID);
        string GetLastMove(string table);
        string GetMovePozition(string table);
        string DeleteTableMove(string table);
        string GetResultСircle();
        string GetResult();
        bool UpdateStatus();
    }
}
