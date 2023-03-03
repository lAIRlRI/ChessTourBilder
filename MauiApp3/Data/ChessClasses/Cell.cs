using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    internal struct Cell
    {
        public int X;
        public int Y;
        public string cell;

        public Cell(string cell) 
        {
            this.cell = cell;
            X = Convert.ToInt32(Helper.StringToInt[cell[0]]);
            Y = Convert.ToInt32(cell[1].ToString());
        }
    }
}
