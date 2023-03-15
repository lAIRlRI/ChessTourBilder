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
            List<Cell> cellsTrue = new List<Cell>();

            if (Pozition.Y == 8) return cells;
            int vector = -1;

            if (IsWhile) vector = 1;

            for (int i = Pozition.X - 1; i <= Pozition.X + 1; i++)
            {
                if (i > 8 || i < 1) continue;
                cells.Add(new(i, Pozition.Y + vector));
            }

            if (!IsMoving)
            {
                Cell cell = new(Pozition.X, Pozition.Y + (vector * 2));
                Cell cellChange = new(Pozition.X, Pozition.Y + vector);
                if (figures.Where(p => p.Pozition.cell == cellChange.cell && p.IsWhile == IsWhile).FirstOrDefault() == default(Figure))
                {
                    cells.Add(cell);
                }
            };

            foreach (var item in cells)
            {
                if (figures.Where(p => p.Pozition.cell == item.cell && p.IsWhile == IsWhile).FirstOrDefault() == default(Figure))
                {
                    cellsTrue.Add(item);
                }
            }

            return cellsTrue.Where(p => p.Y > 0 && p.Y < 9).ToList();
        }

        public override (string, int) SetFigureTrueMove(Figure[] figures, string move, string tableFigures, int orderCaptures, string tableMove)
        {
            (string, int) result;
            result.Item1 = null;
            result.Item2 = orderCaptures;

            string insertMove = Name + move;

            Figure NotGameFigure = figures.Where(p => p.Pozition.cell == move && p.InGame == true).FirstOrDefault();

            string str;

            if (NotGameFigure != default(Figure))
            {
                if (move[0] == Pozition.cell[0]) return result;
                if (NotGameFigure.IsWhile == IsWhile) return result;

                result.Item2++;

                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 0," +
                $" EatID = {result.Item2}" +
                $" WHERE ID = {NotGameFigure.ID}";
                DataBaseFullConn.ConnChange(str);

                insertMove = Name + "x" + move;
            }

            if (move[1] == '8' || move[1] == '1')
            {
                result.Item2++;
                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 0," +
                $" EatID = {result.Item2}" +
                $" WHERE ID = {ID}";
                DataBaseFullConn.ConnChange(str);

                result.Item1 = "rpt";

                return result;
            }

            string temp = Proxod(move, tableMove, figures, tableFigures, result.Item2);

            if(temp != null) 
            {
                result.Item2++;
                insertMove = temp;
            }

            if (move[0] != Pozition.cell[0] && NotGameFigure == default(Figure)) return result;

            str = $"UPDATE {tableFigures} " +
                $"SET Pozition = '{move}'," +
                $"IsMoving = 1 " +
                $" WHERE ID = {ID}";
            DataBaseFullConn.ConnChange(str);

            result.Item1 = insertMove;

            return result;
        }

        private string Proxod(string move, string tableMove, Figure[] figures, string tableFigures, int oreder)
        {
            if (move[0] == Pozition.cell[0]) return null;
            int vectorY = IsWhile ? 7 : 2;
            int vectorE = IsWhile ? 6 : 3;

            if (move[1].ToString() != vector.ToString()) return null;

            DataSet dataSet = DataBaseFullConn.ConnDataSet($"select Move from {tableMove} " +
               "where ID in " +
               $"(select top 1 ID from {tableMove} order by ID desc)");
            string moveChange = dataSet.Tables[0].Rows[0][0].ToString();

            if (moveChange != $"{move[0]}{vectorE}") return null;

            dataSet = DataBaseFullConn.ConnDataSet($"select Move from {tableMove} " +
               $"where Move = {move[0]}{vectorY}");
            if (!dataSet.IsNull("Move")) return false;

            moveChange = $"{move[0]}{vectorE}";

            Figure figure = figures.Where(p => p.Pozition.cell == moveChange).FirstOrDefault();

            if (figure == default(Figure)) return null;
            oreder++;
            string str = $"UPDATE {tableFigures} " +
            $"SET InGame = 0," +
            $" EatID = {oreder}" +
            $" WHERE ID = {figure.ID}";
            DataBaseFullConn.ConnChange(str);

            return Name + "x" + move;
        }
    }
}