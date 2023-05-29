using ChessTourBuilderApp.Data.Controler.ControlerServer;
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

        public virtual async Task<(string, int)> SetFigure(Figure[] figures, string move, string tableFigures, int orderCaptures, string tableMove)
        {
            (string, int) result;
            result.Item1 = null;
            result.Item2 = orderCaptures;

            string realMove = Move(new Cell(move), figures);
            if (realMove == null) return result;

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

                await StaticResouses.mainControler.FigureTableControler.UpdateEat(tableFigures, updateFigureModel);

                insertMove = Name + "x" + move;
            }

            updateFigureModel = new()
            {
                Item1 = move,
                Item2 = ID.ToString()
            };

            await StaticResouses.mainControler.FigureTableControler.UpdatePozition(tableFigures, true, updateFigureModel);

            result.Item1 = insertMove;

            return result;
        }
    }
}