using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    internal abstract class Figure
    {
        public abstract string Name { get; }
        public Cell Pozition { get; set; }
        public bool IsWhile { get; set; }
        public int ID { get; }
        public bool InGame { get; set; } = true;


        public Figure(string pozition, bool isWhile, int id)
        {
            Pozition = new(pozition);
            IsWhile = isWhile;
            ID = id;
        }

        public abstract bool Move(Cell move, Figure[] figures, Cell pozition);
    }
}
