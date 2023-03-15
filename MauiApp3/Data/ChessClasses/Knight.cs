using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Конь
    ///</summary>
    internal class Knight : Figure
    {
        public override string Name { get; } = "N";

        public Knight(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override string Move(Cell move, Figure[] figures)
        {
            if (Math.Pow(move.X - Pozition.X, 2) + Math.Pow(move.Y - Pozition.Y, 2) != 5) return null;
            return move.cell;
        }



        public override List<Cell> GetCells(Figure[] figures)
        {
            List<Cell> cells = new List<Cell>();
            List<Cell> cellsTrue = new List<Cell>();

            if (Pozition.X - 2 > 0) 
            {                
                if (Pozition.Y + 1 < 9) cells.Add(new Cell(Pozition.X - 2, Pozition.Y + 1));

                if (Pozition.Y - 1 > 0) cells.Add(new Cell(Pozition.X - 2, Pozition.Y - 1));
            }

            if (Pozition.X - 1 > 0)
            {
                if (Pozition.Y + 2 < 9) cells.Add(new Cell(Pozition.X - 1, Pozition.Y + 2));

                if (Pozition.Y - 2 > 0) cells.Add(new Cell(Pozition.X - 1, Pozition.Y - 2));
            }

            if (Pozition.X + 1 < 9)
            {
                if (Pozition.Y + 2 < 9) cells.Add(new Cell(Pozition.X + 1, Pozition.Y + 2));

                if (Pozition.Y - 2 > 0) cells.Add(new Cell(Pozition.X + 1, Pozition.Y - 2));
            }

            if (Pozition.X + 2 < 9)
            {
                if (Pozition.Y + 1 < 9) cells.Add(new Cell(Pozition.X + 2, Pozition.Y + 1));

                if (Pozition.Y - 1 > 0) cells.Add(new Cell(Pozition.X + 2, Pozition.Y - 1));
            }

            foreach (var item in cells)
            {
                if (figures.Where(p => p.Pozition.cell == item.cell && p.IsWhile == IsWhile).FirstOrDefault() == default(Figure)) cellsTrue.Add(item);
            }

            return cellsTrue;
        }
    }
}