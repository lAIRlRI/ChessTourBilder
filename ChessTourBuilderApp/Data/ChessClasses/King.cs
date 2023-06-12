using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class King : Figure
    {
        public override string Name { get; } = "K";

        UpdateFigureModel updateFigureModel;

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

        public override async Task<(string, int)> SetFigure(Figure[] figures, string move, string tableFigures, int orderCaptures, string tableMove)
        {
            (string, int) result;
            result.Item1 = null;
            result.Item2 = orderCaptures;

            string realMove = Move(new Cell(move), figures);
            if (realMove == null) return result;

            string insertMove = Name + move;

            if (realMove == "O-O-O")
            {
                updateFigureModel = new()
                {
                    Item1 = $"C{Pozition.Y}",
                    Item2 = ID.ToString()
                };

                await StaticResouses.mainControler.FigureTableControler.UpdatePozition(tableFigures, true, updateFigureModel);

                updateFigureModel = new()
                {
                    Item1 = $"D{Pozition.Y}",
                    Item2 = (IsWhile ? 31 : 32).ToString()
                };

                await StaticResouses.mainControler.FigureTableControler.UpdatePozition(tableFigures, true, updateFigureModel);

                result.Item1 = "O-O-O";
                return result;
            }

            if (realMove == "O-O")
            {
                updateFigureModel = new()
                {
                    Item1 = $"G{Pozition.Y}",
                    Item2 = ID.ToString()
                };

                await StaticResouses.mainControler.FigureTableControler.UpdatePozition(tableFigures, true, updateFigureModel);

                updateFigureModel = new()
                {
                    Item1 = $"F{Pozition.Y}",
                    Item2 = (IsWhile ? 29 : 30).ToString()
                };

                await StaticResouses.mainControler.FigureTableControler.UpdatePozition(tableFigures, true, updateFigureModel);

                result.Item1 = "O-O";
                return result;
            }

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
            Figure rook;

            if (count)
            {
                rook = figures.FirstOrDefault(p => p.Pozition.Y == Convert.ToInt32(poz) && p.IsWhile == IsWhile && p.Pozition.X == 8 && p.InGame == true);
                if (rook == null) return false;
                if (rook.Name != "R") return false;
                if (rook.IsMoving) return false;
                cells = new Cell[2] { new Cell("F" + poz), new Cell("G" + poz) };
            }
            else
            {
                rook = figures.FirstOrDefault(p => p.Pozition.Y == Convert.ToInt32(poz) && p.IsWhile == IsWhile && p.Pozition.X == 1 && p.InGame == true);
                if (rook == null) return false;
                if (rook.Name != "R") return false;
                if (rook.IsMoving) return false;
                cells = new Cell[3] { new Cell("B" + poz), new Cell("C" + poz), new Cell("D" + poz) };
            }

            foreach (var item in cells)
            {
                if (figures.FirstOrDefault(p => p.Pozition.cell == item.cell && p.InGame == true) != default(Figure)) return false;
            }

            foreach (var cell in cells)
            {
                foreach (var figure in figures.Where(p => p.IsWhile != IsWhile && p.InGame == true))
                {
                    if (figure.Move(cell, figures) != null) return false;
                }
            }

            return true;
        }

        public override List<Cell> GetCells(Figure[] figures)
        {
            List<Cell> cells = new();

            for (int i = Pozition.X - 1; i <= Pozition.X + 1; i++)
            {
                for (int j = Pozition.Y - 1; j <= Pozition.Y + 1; j++)
                {
                    if (Pozition.X == i && Pozition.Y == j) continue;
                    cells.Add(new Cell(i, j));
                }
            }

            cells = cells.Where(p => p.X > 0 && p.Y > 0 && p.X < 9 && p.Y < 9).ToList();

            List<Cell> cellsTrue = new();

            foreach (var item in cells)
            {
                if (ChangePozition(item, figures)) cellsTrue.Add(item);
            }

            string[] str = Castling(figures);

            if (str != null)
            {
                if (str[0] == "O-O") cellsTrue.Add(new Cell("H" + Pozition.Y.ToString()));
                else if (str[1] == "O-O-O") cellsTrue.Add(new Cell("A" + Pozition.Y.ToString()));
            }

            return cellsTrue;
        }

        public bool ChangePozition(Cell figure, Figure[] figures)
        {
            if (figures.FirstOrDefault(p => p.Pozition.cell == figure.cell && p.IsWhile == IsWhile && p.InGame == true) != default(Figure)) return false;

            foreach (var item in figures.Where(p => p.IsWhile != IsWhile && p.InGame == true))
            {
                if (item.Move(figure, figures) != null) return false;
            }

            return true;
        }
    }
}