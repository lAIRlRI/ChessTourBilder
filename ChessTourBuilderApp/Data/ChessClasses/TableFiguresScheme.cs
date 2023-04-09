using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class TableFiguresScheme
    {
        public static readonly Func<IDataReader, TableFiguresScheme> mapper = r => new TableFiguresScheme()
        {
            Move = r["Move"].ToString(),
            Pozition = r["Pozition"].ToString()
        };

        public string Move { get; set; }
        public string Pozition { get; set; }

        public TableFiguresScheme(string move, string pozition)
        {
            Move = move;
            Pozition = pozition;
        }

        public TableFiguresScheme()
        {

        }
    }
}