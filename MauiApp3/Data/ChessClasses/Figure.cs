using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    internal abstract class Figure
    {
        public abstract string Name { get; }
        public Cell Pozition { get; set; }
        public bool IsWhile { get; set; }
        public int ID { get; }
        public bool InGame { get; set; } = true;
        public bool IsMoving { get; set; } = false;


        public Figure(string pozition, bool isWhile, int id)
        {
            Pozition = new(pozition);
            IsWhile = isWhile;
            ID = id;
        }

        public abstract string Move(Cell move, Figure[] figures);

        public abstract List<Cell> GetCells(Figure[] figures);

        public string SetFigure(Figure[] figures, string move, string tableFigures, int orderCaptures) 
        {
            string realMove = Move(new Cell(move), figures);
            if (realMove == null) return realMove;
            string insertMove = Name + move;

            Figure NotGameFigure = figures.Where(p => p.Pozition.cell == move && p.InGame == true).FirstOrDefault();

            string str;

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

        public virtual string SetFigureTrueMove(Figure[] figures, string move, string tableFigures, int orderCaptures)
        {
            string insertMove = Name + move;

            Figure NotGameFigure = figures.Where(p => p.Pozition.cell == move && p.InGame == true).FirstOrDefault();

            string str;

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
                $"SET Pozition = '{move}'," +
                $"IsMoving = 1" +
                $" WHERE ID = {ID}";
            DataBaseFullConn.ConnChange(str);

            return insertMove;
        }
    }
}
