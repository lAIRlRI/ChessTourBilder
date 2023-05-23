using ChessTourBuilderApp.Data.Controler;
using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal abstract class Figure
    {
        public abstract string Name { get; }
        public Cell Pozition { get; set; }
        public bool IsWhile { get; set; }
        public int ID { get; }
        public bool InGame { get; set; } = true;
        public bool IsMoving { get; set; } = false;
        UpdateFigureModel updateFigureModel;

        public Figure(string pozition, bool isWhile, int id)
        {
            Pozition = new(pozition);
            IsWhile = isWhile;
            ID = id;
        }

        public abstract string Move(Cell move, Figure[] figures);

        public abstract List<Cell> GetCells(Figure[] figures);

        public async Task<string> SetFigure(Figure[] figures, string move, string tableFigures, int orderCaptures)
        {
            string realMove = Move(new Cell(move), figures);
            if (realMove == null) return realMove;
            string insertMove = Name + move;

            Figure NotGameFigure = figures.FirstOrDefault(p => p.Pozition.cell == move && p.InGame == true);

            if (NotGameFigure != default(Figure))
            {
                if (NotGameFigure.IsWhile == IsWhile) return null;

                orderCaptures++;

                updateFigureModel = new()
                {
                    Item1 = orderCaptures.ToString(),
                    Item2 = NotGameFigure.ID.ToString()
                };
                await FigureTableControler.UpdateEat(tableFigures, updateFigureModel);

                insertMove = Name + "x" + move;
            }

            updateFigureModel = new()
            {
                Item1 = move,
                Item2 = ID.ToString()
            };

            await FigureTableControler.UpdatePozition(tableFigures, true, updateFigureModel);

            return insertMove;
        }

        public virtual async Task<(string, int)> SetFigureTrueMove(Figure[] figures, string move, string tableFigures, int orderCaptures, string tableMove)
        {
            (string, int) result;
            result.Item1 = null;
            result.Item2 = orderCaptures;

            string insertMove = Name + move;

            Figure NotGameFigure = figures.FirstOrDefault(p => p.Pozition.cell == move && p.InGame == true);

            if (NotGameFigure != default(Figure))
            {
                if (NotGameFigure.IsWhile == IsWhile) return result;
                result.Item2++;

                updateFigureModel = new()
                {
                    Item1 = result.Item2.ToString(),
                    Item2 = NotGameFigure.ID.ToString()
                };

                await FigureTableControler.UpdateEat(tableFigures, updateFigureModel);

                insertMove = Name + "x" + move;
            }

            updateFigureModel = new()
            {
                Item1 = move,
                Item2 = ID.ToString()
            };

            await FigureTableControler.UpdatePozition(tableFigures,true, updateFigureModel);

            result.Item1 = insertMove;

            return result;
        }
    }
}