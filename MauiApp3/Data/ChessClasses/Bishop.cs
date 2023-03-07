using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Слон
    ///</summary>
    internal class Bishop : Figure
    {
        public override string Name { get; } = "B";

        private Cell[] cellsHorizontal, cellsVertical;

        public Bishop(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override bool Move(Cell move, Figure[] figures, Cell pozition)
        {
            if (pozition.Y == pozition.Y && pozition.X == move.X) return false;

            if (pozition.X - pozition.Y == move.X - move.Y)
            {
                GetCellsHorizontal(pozition);

                if (cellsHorizontal.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
                {
                    if (ChangePozition(move, pozition, cellsHorizontal, figures)) return true;
                }
            }

            if (Math.Abs(pozition.X - 9) - Math.Abs(pozition.Y - 9) == Math.Abs(move.X - 9) - Math.Abs(move.Y - 9)) return false;
            {
                GetCellsVertical(pozition);

                if (cellsVertical.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
                {
                    if (ChangePozition(move, pozition, cellsVertical, figures)) return true;
                }
            }

            return false;
        }

        public void GetCellsHorizontal(Cell pozition)
        {
            cellsHorizontal = GetCells(pozition);
        }

        private Cell[] GetCells(Cell pozition) 
        {
            Cell[] cells = new Cell[8 - Math.abs(pozition.Y - pozition.X)];

            Cell start;

            if (pozition.X < pozition.Y)
                start = new Cell(1, pozition.Y - pozition.X + 1);
            else start = new Cell(pozition.X - pozition.Y + 1, 1);

            for (int i = 0; i < 8 - cells.length; i++)
            {
                cells[i] = new Cell(start.X + i, start.Y + i);
            }

            return cells;
        }

        public void GetCellsVertical(Cell pozition)
        {
            Cell cell = new(Math.Abs(pozition.X - 9), Math.Abs(pozition.Y - 9));
            cellsVertical = GetCells(cell);

            foreach (var item in cellsVertical)
            {
                item.X = Math.Abs(item.X - 9);
                item.Y = Math.Abs(item.Y - 9);
            }
        }

        private bool ChangePozition(Cell move, Cell pozition, Cell[] cells, Figure[] figures)
        {
            Cell[] cellsDiapozon;
            if (move < pozition)
            {
                cellsDiapozon = cells.Where(p => p < pozition && p > move).ToArray();
            }
            else cellsDiapozon = cells.Where(p => p > pozition && p < move).ToArray();

            foreach (var item in cellsDiapozon)
            {
                if (figures.Where(p => p.Pozition.cell == item.cell) != default(Figure)) return false;
            }

            return true;
        }
    }
}