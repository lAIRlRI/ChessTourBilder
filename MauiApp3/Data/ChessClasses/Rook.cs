using MauiApp3.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Ладья
    ///</summary>
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

            if (cellsHorizontal.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
            {
                if(ChangePozition(move, cellsHorizontal, figures)) return move.cell;
            }

            GetCellsVertical();

            if (cellsVertical.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
            {
                if(ChangePozition(move, cellsVertical, figures)) return move.cell;
            }

            return null;
        }

        public override List<Cell> GetCells(Figure[] figures) 
        {
            GetCellsVertical();
            GetCellsHorizontal();

            Cell[] cells = cellsHorizontal.Concat(cellsVertical).ToArray();
            List<Cell> cellsTrue = new List<Cell>();

            foreach (var item in cells)
            {
                if (ChangePozition(item, cells, figures)) cellsTrue.Add(item);

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

            if (move < Pozition)
            {
                cellsDiapozon = cells.Where(p => p < Pozition && p > move).ToArray();
            }
            else cellsDiapozon = cells.Where(p => p > Pozition && p < move).ToArray();

            foreach (var item in cellsDiapozon)
            {
                if (figures.Where(p => p.Pozition.cell == item.cell).FirstOrDefault() != default(Figure)) return false;
            }

            return true;
        }
    }
}