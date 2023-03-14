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

        public Pawn(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override string Move(Cell move, Figure[] figures)
        {
            if (Pozition.cell == move.cell) return null;
            int vector = -1;
            if (IsWhile) vector = 1;

            if (Pozition.X + 1 == move.X && Pozition.Y + vector == move.Y) return move.cell;
            if (Pozition.X - 1 == move.X && Pozition.Y + vector == move.Y) return move.cell;
            if (Pozition.X == move.X && Pozition.Y + vector == move.Y) return move.cell;
            if (Pozition.X == move.X && Pozition.Y + vector * 2 == move.Y) return move.cell;

            return null;
        }

        public override List<Cell> GetCells(Figure[] figures)
        {


            List<Cell> cells = new List<Cell>();
            if (Pozition.Y == 8) return cells;
            int vector = -1;


            if (IsWhile) vector = 1;

            for (int i = Pozition.X - 1; i <= Pozition.X + 1; i++)
            {
                if (i > 8 || i < 1) continue;
                cells.Add(new Cell(i, Pozition.Y + vector));
            }

            if (!IsMoving) cells.Add(new Cell(Pozition.X, Pozition.Y + (vector * 2)));

            return cells.Where(p => p.Y > 0 && p.Y < 9).ToList();
        }

        public override string SetFigureTrueMove(Figure[] figures, string move, string tableFigures, int orderCaptures)
        {
            string insertMove = Name + move;

            Figure NotGameFigure = figures.Where(p => p.Pozition.cell == move && p.InGame == true).FirstOrDefault();

            string str;

            if (NotGameFigure != default(Figure))
            {
                if (move[0] == Pozition.cell[0]) return null;
                if (NotGameFigure.IsWhile == IsWhile) return null;

                orderCaptures++;
                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 0," +
                $" EatID = {orderCaptures}" +
                $" WHERE ID = {NotGameFigure.ID}";
                DataBaseFullConn.ConnChange(str);
                insertMove = Name + "x" + move;
            }

            if (move[1] == '8' || move[1] == '1')
            {
                orderCaptures++;
                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 0," +
                $" EatID = {orderCaptures}" +
                $" WHERE ID = {ID}";
                DataBaseFullConn.ConnChange(str);
                return "rpt";
            }

            if (move[0] != Pozition.cell[0] && NotGameFigure == default(Figure)) return null;

            str = $"UPDATE {tableFigures} " +
                $"SET Pozition = '{move}'," +
                $"IsMoving = 1 " +
                $" WHERE ID = {ID}";
            DataBaseFullConn.ConnChange(str);

            return insertMove;
        }

    }
}