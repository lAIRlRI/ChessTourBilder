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

        public override bool Move(Cell pozition, Figure[] figures, Cell move)
        {
            if (Math.Pow(move.X - pozition.X, 2) + Math.Pow(move.Y - pozition.Y, 2) != 5) return false;
            return true;
        }

        private Cell[] GetCells(Cell pozition)
        {
            List<Cell> cells = new List<Cell>();

            if (pozition.X + 2 < 8)
            {
                if (pozition.Y + 2 < 8)
                {
                    Cell.add(new Cell(pozition.X + 1, pozition.Y + 2));
                    Cell.add(new Cell(pozition.X + 2, pozition.Y + 1));
                }

                if (pozition.Y - 2 > 0) 
                {
                    Cell.add(new Cell(pozition.X + 2, pozition.Y - 1));
                    Cell.add(new Cell(pozition.X + 1, pozition.Y - 2));
                }
            }

            if (pozition.X - 2 > 0) 
            {
                if (pozition.Y + 2 < 8)
                {
                    Cell.add(new Cell(pozition.X - 2, pozition.Y + 1));
                    Cell.add(new Cell(pozition.X - 1, pozition.Y + 2));
                }
                
                if (pozition.Y - 2 > 0) 
                {
                    Cell.add(new Cell(pozition.X - 2, pozition.Y - 1));
                    Cell.add(new Cell(pozition.X - 1, pozition.Y - 2));
                }
            }

            return cells.ToArray();
        }
    }
}