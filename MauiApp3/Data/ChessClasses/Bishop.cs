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

        private List<Cell> cellsHorizontal, cellsVertical;

        public Bishop(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override string Move(Cell move, Figure[] figures)
        {
            if (Pozition.Y == move.Y && Pozition.X == move.X) return null;

            if (Pozition.X - Pozition.Y == move.X - move.Y)
            {
                GetCellsHorizontal();

                if (cellsHorizontal.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
                {
                    if (ChangePozition(move, cellsHorizontal, figures)) return move.cell;
                }
            }

            if (Pozition.X - Math.Abs(Pozition.Y - 9) == move.X- Math.Abs(move.Y - 9)) 
            { 
                GetCellsVertical();

                if (cellsVertical.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
                {
                    if (ChangePozition(move, cellsVertical, figures)) return move.cell;
                }
            }

            return null;
        }

        public override List<Cell> GetCells(Figure[] figures) 
        {
            GetCellsVertical();
            GetCellsHorizontal();

            List<Cell> cellsTrue = new List<Cell>();

            foreach (var item in cellsHorizontal)
            {
                if (Pozition.X == item.X && Pozition.Y == item.Y) continue;
                if (ChangePozition(item, cellsHorizontal, figures)) cellsTrue.Add(item);
            }

            foreach (var item in cellsVertical)
            {
                if (Pozition.X == item.X && Pozition.Y == item.Y) continue;
                if (ChangePozition(item, cellsVertical, figures)) cellsTrue.Add(item);
            }

            return cellsTrue;
        }

        public void GetCellsHorizontal()
        {
            cellsHorizontal = GetCellsHV(Pozition);
        }

        private List<Cell> GetCellsHV(Cell cell) 
        {
            List<Cell> cells = new List<Cell>();

            Cell start;

            if (cell.X < cell.Y)
                start = new Cell(1, cell.Y - cell.X + 1);
            else start = new Cell(cell.X - cell.Y + 1, 1);

            for (int i = 0; i < 8 - Math.Abs(cell.Y - cell.X); i++)
            {
                cells.Add(new Cell(start.X + i, start.Y + i));
            }

            return cells;
        }

        public void GetCellsVertical()
        {
            Cell cell = new(Pozition.X, Math.Abs(Pozition.Y-9));
            cellsVertical = GetCellsHV(cell);

            foreach (var item in cellsVertical)
            {
                item.Y =  Math.Abs(item.Y - 9);
                item.cell =  Cell.GetString(item.X,item.Y);
            }
        }

        private bool ChangePozition(Cell move, List<Cell> cells, Figure[] figures)
        {
            Cell[] cellsDiapozon;

            if (figures.Where(p => p.Pozition.cell == move.cell && p.IsWhile == IsWhile).FirstOrDefault() != default(Figure)) return false;

            if (move.Y < Pozition.Y)
            {
                cellsDiapozon = cells.Where(p => p.Y < Pozition.Y && p.Y > move.Y).ToArray();
            }
            else cellsDiapozon = cells.Where(p => p.Y > Pozition.Y && p.Y < move.Y).ToArray();

            foreach (var item in cellsDiapozon)
            {
                if (figures.Where(p => p.Pozition.cell == item.cell).FirstOrDefault() != default(Figure)) return false;
            }

            return true;
        }
    }
}