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
        public override string Name { get; } = "K";
        public King(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override string Move(Cell move, Figure[] figures)
        {
            if ("A" + Pozition.Y.ToString() == move.cell)
            {
                if (ChangeCastling(move.cell, false, figures)) return "O-O-O";
            }

            if ("H" + Pozition.Y.ToString() == move.cell)
            {
                if (ChangeCastling(move.cell, true, figures)) return "O-O";
            }

            if (Math.Abs(Pozition.X - move.X) > 1 || Math.Abs(Pozition.Y - move.Y) > 1) return null;

            bool str = ChangePozition(move, figures);

            return str ? move.cell : null;
        }

        public new string SetFigure(Figure[] Figures, string move, string tableFigures, int orderCaptures)
        {
            string str;
            string realMove = Move(new Cell(move), Figures);
            if (realMove == null) return realMove;
            string insertMove = Name + move;

            if (realMove == "O-O-O")
            {
                str = $"UPDATE {tableFigures} " +
                      $"SET Pozition = 'C{Pozition.Y}'" +
                      $" WHERE ID = {ID}";
                DataBaseFullConn.ConnChange(str);

                str = $"UPDATE {tableFigures} " +
                      $"SET Pozition = 'D{Pozition.Y}'" +
                      $" WHERE Figure = 'R' and Pozition = 'A{Pozition.Y}'";
                DataBaseFullConn.ConnChange(str);

                return realMove;
            }

            if (realMove == "O-O")
            {
                str = $"UPDATE {tableFigures} " +
                      $"SET Pozition = 'G{Pozition.Y}'" +
                      $" WHERE ID = {ID}";
                DataBaseFullConn.ConnChange(str);

                str = $"UPDATE {tableFigures} " +
                      $"SET Pozition = 'F{Pozition.Y}'" +
                      $" WHERE Figure = 'R' and Pozition = 'H{Pozition.Y}'";
                DataBaseFullConn.ConnChange(str);

                return realMove;
            }

            Figure NotGameFigure = Figures.Where(p => p.Pozition.cell == move && p.InGame == true).FirstOrDefault();

            if (NotGameFigure != default(Figure))
            {
                if (NotGameFigure.IsWhile == IsWhile) return null;

                orderCaptures++;
                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 0," +
                $" EatID = {orderCaptures}" +
                $" WHERE ID = {NotGameFigure.ID}";
                DataBaseFullConn.ConnChange(str);
                insertMove = Name + "x" + move;

            }

            str = $"UPDATE {tableFigures} " +
                $"SET Pozition = '{move}'" +
                $" WHERE ID = {ID}";
            DataBaseFullConn.ConnChange(str);

            return insertMove;
        }

        public new string SetFigureTrueMove(Figure[] figures, string move, string tableFigures, int orderCaptures)
        {
            string str;
            string insertMove = Name + move;

            if (move == $"A{Pozition.Y}")
            {
                str = $"UPDATE {tableFigures} " +
                      $"SET Pozition = 'C{Pozition.Y}'" +
                      $" WHERE ID = {ID}";
                DataBaseFullConn.ConnChange(str);

                str = $"UPDATE {tableFigures} " +
                      $"SET Pozition = 'D{Pozition.Y}'" +
                      $" WHERE Figure = 'R' and Pozition = 'A{Pozition.Y}'";
                DataBaseFullConn.ConnChange(str);

                return "O-O-O";
            }

            if (move == $"H{Pozition.Y}")
            {
                str = $"UPDATE {tableFigures} " +
                      $"SET Pozition = 'G{Pozition.Y}'" +
                      $" WHERE ID = {ID}";
                DataBaseFullConn.ConnChange(str);
                 
                str = $"UPDATE {tableFigures} " +
                      $"SET Pozition = 'F{Pozition.Y}'" +
                      $" WHERE Figure = 'R' and Pozition = 'H{Pozition.Y}'";
                DataBaseFullConn.ConnChange(str);

                return "O-O";
            }

            Figure NotGameFigure = figures.Where(p => p.Pozition.cell == move && p.InGame == true).FirstOrDefault();

            if (NotGameFigure != default(Figure))
            {
                if (NotGameFigure.IsWhile == IsWhile) return null;

                orderCaptures++;
                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 0," +
                $" EatID = {orderCaptures}" +
                $" WHERE ID = {NotGameFigure.ID}";
                DataBaseFullConn.ConnChange(str);
                insertMove = Name + "x" + move;

            }

            str = $"UPDATE {tableFigures} " +
                $"SET Pozition = '{move}'" +
                $" WHERE ID = {ID}";
            DataBaseFullConn.ConnChange(str);

            return insertMove;
        }

        private string[] Castling(Figure[] figures)
        {
            string poz = Pozition.Y.ToString();

            string[] cast = new string[2];

            if (!IsMoving)
            {
                if (ChangeCastling(poz, true, figures)) cast[0] = "O-O";
                if (ChangeCastling(poz, false, figures)) cast[1] = "O-O-O";
            }
            return cast;
        }

        private bool ChangeCastling(string poz, bool count, Figure[] figures)
        {
            Cell[] cells;

            Figure rook = figures.Where(p => p.Pozition.cell == poz && p.IsWhile == IsWhile).FirstOrDefault();

            if (rook == null) return false;
            if (rook.Name != "R") return false;
            if (rook.IsMoving == true) return false;

            if (count)
            {
                cells = new Cell[2] { new Cell("F" + poz), new Cell("G" + poz) };
            }
            else
            {
                cells = new Cell[3] { new Cell("B" + poz), new Cell("C" + poz), new Cell("D" + poz) };
            }

            foreach (var item in cells)
            {
                if (figures.Where(p => p.Pozition.cell == item.cell).FirstOrDefault() != default(Figure)) return false;
            }

            foreach (var cell in cells)
            {
                foreach (var figure in figures.Where(p => p.IsWhile != IsWhile))
                {
                    if (figure.Move(cell, figures) != null) return false;
                }
            }

            return true;
        }

        public override List<Cell> GetCells(Figure[] figures)
        {
            List<Cell> cells = new List<Cell>();

            for (int i = Pozition.X - 1; i <= Pozition.X + 1; i++)
            {
                for (int j = Pozition.Y - 1; j <= Pozition.Y + 1; j++)
                {
                    if (Pozition.X == i && Pozition.Y == j) continue;
                    cells.Add(new Cell(i, j));
                }
            }

            cells = cells.Where(p => p.X > 0 && p.Y > 0 && p.X < 9 && p.Y < 9).ToList();

            List<Cell> cellsTrue = new List<Cell>();

            foreach (var item in cells)
            {
                if (ChangePozition(item, figures)) cellsTrue.Add(item);
            }

            string[] str = Castling(figures);

            if (str != null)
            {
                if (str[0] == "O-O") cellsTrue.Add(new Cell("H" + Pozition.Y.ToString()));
                else if (str[0] == "O-O-O") cellsTrue.Add(new Cell("A" + Pozition.Y.ToString()));
            }

            return cellsTrue;
        }

        public bool ChangePozition(Cell figure, Figure[] figures)
        {
            if (figures.Where(p => p.Pozition.cell == figure.cell && p.IsWhile == IsWhile).FirstOrDefault() != default(Figure)) return false;

            foreach (var item in figures.Where(p => p.IsWhile != IsWhile))
            {
                if (item.Move(figure, figures) != null) return false;
            }

            return true;
        }
    }
}