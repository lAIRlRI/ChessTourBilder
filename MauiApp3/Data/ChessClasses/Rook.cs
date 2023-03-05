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

        public bool IsMoving { get; set; } = false;

        public Rook(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override bool Move(Cell move, Figure[] figures, Cell pozition)
        {
            if (pozition.X != move.X && pozition.Y != move.Y) return false;
            if (pozition.X == move.X && pozition.Y == move.Y) return false;

            GetCells(pozition);

            if (cellsHorizontal.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
            {
                if(GetCellsHorizontal(move, pozition, cellsHorizontal, figures)) return true;
            }

            if (cellsVertical.Where(p => p.cell == move.cell).FirstOrDefault() != default(Cell))
            {
                if(GetCellsVertical(move, pozition, cellsVertical, figures)) return true;
            }

            return false;
        }

        public void GetCellsHorizontal(Cell pozition)
        {
            cellsHorizontal = new Cell[8];

            for (int i = 0; i < 8; i++)
            {
                cellsHorizontal[i] = new Cell(pozition.X, i + 1);
            }
        }

        public void GetCellsVertical(Cell pozition)
        {
            cellsVertical = new Cell[8];

            for (int i = 0; i < 8; i++)
            {
                cellsVertical[i] = new Cell(i + 1, pozition.Y);
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