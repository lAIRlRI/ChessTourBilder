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
            if (pozition.Y == move.Y && pozition.X == move.X) return false;

            if (pozition.X - pozition.Y == move.X - move.Y)
            {
                GetCellsHorizontal(pozition);

                if (cellsHorizontal.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
                {
                    if (ChangePozition(move, pozition, cellsHorizontal, figures)) return true;
                }
            }

            if (pozition.X - Math.Abs(pozition.Y - 9) == move.X- Math.Abs(move.Y - 9)) 
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
            Cell[] cells = new Cell[8 - Math.Abs(pozition.Y - pozition.X)];

            Cell start;

            if (pozition.X < pozition.Y)
                start = new Cell(1, pozition.Y - pozition.X + 1);
            else start = new Cell(pozition.X - pozition.Y + 1, 1);

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new Cell(start.X + i, start.Y + i);
            }

            return cells;
        }

        public void GetCellsVertical(Cell pozition)
        {
            Cell cell = new(pozition.X, Math.Abs(pozition.Y-9));
            cellsVertical = GetCells(cell);

            foreach (var item in cellsVertical)
            {
                item.Y =  Math.Abs(item.Y - 9);
                item.cell =  Cell.GetString(item.X,item.Y);
            }
        }

        private bool ChangePozition(Cell move, Cell pozition, Cell[] cells, Figure[] figures)
        {
            Cell[] cellsDiapozon;

            if (move.Y < pozition.Y)
            {
                cellsDiapozon = cells.Where(p => p.Y < pozition.Y && p.Y > move.Y).ToArray();
            }
            else cellsDiapozon = cells.Where(p => p.Y > pozition.Y && p.Y < move.Y).ToArray();

            foreach (var item in cellsDiapozon)
            {
                if (figures.Where(p => p.Pozition.cell == item.cell).FirstOrDefault() != default(Figure)) return false;
            }

            return true;
        }
    }
}