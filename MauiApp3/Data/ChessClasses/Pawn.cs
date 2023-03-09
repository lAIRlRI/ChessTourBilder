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
        public bool IsMoving { get; set; } = false;

        public Pawn(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override bool Move(Cell pozition, Figure[] figures, Cell move)
        {
            if (pozition.cell = move.cell) return false;
            return true;
        }

        public Cell[] GetCell(Cell pozition)
        {
            List<Cell> cells = new List<Cell>();

            int vector = -1;

            if (IsWhile) vector = 1;

            for (int i = pozition.X - 1; i < 3; i++)
            {
                cells.Add(i, pozition.Y + vector);
            }

            if (IsMoving) cells.Add(pozition.X, pozition.Y + 2);

            return cells.Where(p => p.X > 0 && p.Y > 0 && p.X < 9 && p.Y < 9).ToArray();
        }
    }
}