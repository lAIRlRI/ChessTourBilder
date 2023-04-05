using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class Rook : Figure
    {
        public override string Name { get; } = "R";

        private Cell[] cellsHorizontal, cellsVertical;

        public Rook(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override string Move(Cell move, Figure[] figures)
        {
            if (Pozition.X != move.X && Pozition.Y != move.Y) return null;
            if (Pozition.X == move.X && Pozition.Y == move.Y) return null;

            GetCellsHorizontal();

            if (cellsHorizontal.FirstOrDefault(p => p.cell == move.cell) != default(Cell))
            {
                if (ChangePozition(move, cellsHorizontal, figures)) return move.cell;
            }

            GetCellsVertical();

            if (cellsVertical.FirstOrDefault(p => p.cell == move.cell) != default(Cell))
            {
                if (ChangePozition(move, cellsVertical, figures)) return move.cell;
            }

            return null;
        }

        public override List<Cell> GetCells(Figure[] figures)
        {
            GetCellsVertical();
            GetCellsHorizontal();

            List<Cell> cellsTrue = new();

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
            cellsHorizontal = new Cell[8];

            for (int i = 0; i < 8; i++)
            {
                cellsHorizontal[i] = new Cell(Pozition.X, i + 1);
            }
        }

        public void GetCellsVertical()
        {
            cellsVertical = new Cell[8];

            for (int i = 0; i < 8; i++)
            {
                cellsVertical[i] = new Cell(i + 1, Pozition.Y);
            }
        }

        private bool ChangePozition(Cell move, Cell[] cells, Figure[] figures)
        {
            Cell[] cellsDiapozon;

            if (figures.FirstOrDefault(p => p.Pozition.cell == move.cell && p.IsWhile == IsWhile) != default(Figure)) return false;

            if (move < Pozition)
            {
                cellsDiapozon = cells.Where(p => p < Pozition && p > move).ToArray();
            }
            else cellsDiapozon = cells.Where(p => p > Pozition && p < move).ToArray();

            foreach (var item in cellsDiapozon)
            {
                if (figures.FirstOrDefault(p => p.Pozition.cell == item.cell) != default(Figure)) return false;
            }

            return true;
        }
    }
}
