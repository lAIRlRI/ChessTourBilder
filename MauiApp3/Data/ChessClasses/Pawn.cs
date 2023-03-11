using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Пешка
    ///</summary>
    internal class Pawn : Figure
    {
        public override string Name { get; } = "";

        public Pawn(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override string Move(Cell move, Figure[] figures)
        {
            if (Pozition.cell == move.cell) return null;
            return move.cell;
        }

        public override List<Cell> GetCells(Figure[] figures)
        {


            List<Cell> cells = new List<Cell>();
            if (Pozition.Y == 8) return cells;
            int vector = -1;


            if (IsWhile) vector = 1;

            for (int i = Pozition.X - 1; i <= Pozition.X + 1; i++)
            {
                if (i > 8 || i < 1) continue;
                cells.Add(new Cell(i, Pozition.Y + vector));
            }

            if (!IsMoving) cells.Add(new Cell(Pozition.X, Pozition.Y + 2));

            return cells.Where(p => p.Y > 0 && p.Y < 9).ToList();
        }
    }
}