using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Король
    ///</summary>
    internal class King : Figure
    {
        public override string Name { get;} = "K";
        public bool IsMoving { get; set; } = false;
        public King(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override bool Move(Cell pozition, Figure[] figures, Cell move)
        {
            if (Math.Abs(pozition.X - move.X) > 1 || Math.Abs(pozition.Y - move.Y) > 1) return false;
            //if (Math.Pow(move.X - pozition.X, 2) + Math.Pow(move.Y - pozition.Y, 2) != 2) return false;
            //if (Math.Pow(move.X - pozition.X, 2) + Math.Pow(move.Y - pozition.Y, 2) != 1) return false;

            return true;
        }

        public Cell[] GetCells(Cell pozition) 
        {
            List<Cell> cells = new List<Cell>();

            for (int i = pozition.X-1; i < 3; i++)
            {
                for (int j = pozition.Y-1; j < 3; j++)
                {
                    cells.Add(i, j);
                }
            }

            cells = cells.Where(p => p.X > 0 && p.Y > 0 && p.X < 9 && p.Y < 9).ToList();

            return cells.ToArray();
        }

        public Cell[] GetCells()
        {
            List<Cell> cells = new List<Cell>();

            for (int i = Pozition.X - 1; i < 3; i++)
            {
                for (int j = Pozition.Y - 1; j < 3; j++)
                {
                    if (Pozition.X == i && Pozition.Y == j) continue;
                    cells.Add(i, j);
                }
            }

            cells = cells.Where(p => p.X > 0 && p.Y > 0 && p.X < 9 && p.Y < 9).ToList();

            return cells.ToArray();
        }
    }
}