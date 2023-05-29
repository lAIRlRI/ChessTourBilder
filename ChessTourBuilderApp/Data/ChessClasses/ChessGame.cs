using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using System.Data;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class ChessGame
    {
        private readonly string tableMove = "[" + StaticResouses.mainControler.EventControler.nowEvent.Name + DateTime.UtcNow + "]";
        private readonly string tableFigures = "[Figures" + DateTime.UtcNow + "]";
        private readonly char[] figure = new char[] { 'Q', 'N', 'B', 'R' };

        private Consignment consignment;
        private Cell lastPozition;
        private int orderCaptures = 0;
        private int lastIDFigure;

        public Figure[] Figures { get; private set; }
        public List<string> Move { get; } = new List<string>();
        public bool IsGameContinues { get; private set; } = true;

        public ChessGame(Consignment consignment)
        {
            this.consignment = consignment;
        }

        public async Task InizializeGame() 
        {
            await StaticResouses.mainControler.MoveTableControler.CreateTableMove(tableMove);

            consignment.TableName = tableMove;

            await StaticResouses.mainControler.ConsignmentControler.Update(consignment, consignment.ConsignmentID);

            if (await StaticResouses.mainControler.FigureTableControler.CreateFigureMove(tableFigures)) 
            {
               await GetFigures();
            }
        }

        public async Task GetFigures()
        {
            var table = await StaticResouses.mainControler.FigureTableControler.GetAll(tableFigures);
            int i = 0;
            Figures = new Figure[table.Count];

            foreach (var item in table)
            {
                switch (item.Figure)
                {
                    case "":
                        Figures[i] = new Pawn(item.Pozition, item.IsWhile, item.ID)
                        {
                            InGame = item.InGame,
                            IsMoving = item.IsMoving
                        };
                        break;
                    case "K":
                        Figures[i] = new King(item.Pozition, item.IsWhile, item.ID)
                        {
                            InGame = item.InGame,
                            IsMoving = item.IsMoving
                        };
                        break;
                    case "Q":
                        Figures[i] = new Queen(item.Pozition, item.IsWhile, item.ID)
                        {
                            InGame = item.InGame,
                            IsMoving = item.IsMoving
                        };
                        break;
                    case "B":
                        Figures[i] = new Bishop(item.Pozition, item.IsWhile, item.ID)
                        {
                            InGame = item.InGame,
                            IsMoving = item.IsMoving
                        };
                        break;
                    case "N":
                        Figures[i] = new Knight(item.Pozition, item.IsWhile, item.ID)
                        {
                            InGame = item.InGame,
                            IsMoving = item.IsMoving
                        };
                        break;
                    case "R":
                        Figures[i] = new Rook(item.Pozition, item.IsWhile, item.ID)
                        {
                            InGame = item.InGame,
                            IsMoving = item.IsMoving
                        };
                        break;
                }
                i++;
            }
        }

        public async Task<string> SetFigure(Figure figure, string move)
        {
            if (figure == null) return "no";

            (string, int) function = await figure.SetFigure(Figures, move, tableFigures, orderCaptures, tableMove);

            string insertMove = function.Item1;
            orderCaptures = function.Item2;

            if (insertMove == null) return "no";
            if ("rpt" == insertMove) return "rpt";

            int ID = figure.IsWhile ? consignment.whitePlayer.PlayerID : consignment.blackPlayer.PlayerID;

            MoveTableModel moveTableModel = new()
            {
                ID = ID,
                Move = insertMove,
                ConsignmentID = consignment.ConsignmentID,
                TourID = consignment.TourID,
                Pozition = figure.Pozition.cell
            };

            await StaticResouses.mainControler.MoveTableControler.PostMove(tableMove, moveTableModel);

            lastPozition = figure.Pozition;
            lastIDFigure = figure.ID;

            Move.Add(insertMove);

            return "ok";
        }

        public async Task<bool> InsertFigure(string pozition, string name, bool IsWhile)
        {
            FigureScheme figureScheme = new() 
            {
                Figure = name, 
                Pozition = pozition,
                IsWhile = IsWhile
            };
            await StaticResouses.mainControler.FigureTableControler.InsertFigure(tableFigures, figureScheme);

            int ID = IsWhile ? consignment.whitePlayer.PlayerID : consignment.blackPlayer.PlayerID;

            MoveTableModel moveTableModel = new()
            {
                ID = ID,
                Move = pozition + name,
                ConsignmentID = consignment.ConsignmentID,
                TourID = consignment.TourID,
                Pozition = pozition + name
            };
            await StaticResouses.mainControler.MoveTableControler.PostMove(tableMove, moveTableModel);

            await GetFigures();
            Move.Add(pozition + name);
            return true;
        }

        public Figure[] GetFigure(bool IsWhile) 
        {
             return Figures.Where(p => p.IsWhile == IsWhile && p.InGame == true).ToArray();
        }

        public async void EndGame(double? result)
        {
            await StaticResouses.mainControler.MoveTableControler.PutLastMove(tableMove);

            if (result == 2)
            {
                consignment.whitePlayer.Result = 0.5;
                consignment.blackPlayer.Result = 0.5;
            }
            else
            {
                int ID;
                if (result == 1)
                {
                    ID = consignment.whitePlayer.PlayerID;
                    consignment.whitePlayer.Result = 1;
                    consignment.blackPlayer.Result = 0;
                }
                else
                {
                    ID = consignment.blackPlayer.PlayerID;
                    consignment.whitePlayer.Result = 0;
                    consignment.blackPlayer.Result = 1;
                }

                await StaticResouses.mainControler.MoveTableControler.PutWinner(tableMove, ID);
            }

            consignment.whitePlayer.player.ELORating = ELO((double)consignment.whitePlayer.player.ELORating, (double)consignment.blackPlayer.player.ELORating, (double)consignment.whitePlayer.Result);
            consignment.blackPlayer.player.ELORating = ELO((double)consignment.blackPlayer.player.ELORating, (double)consignment.whitePlayer.player.ELORating, (double)consignment.blackPlayer.Result);

            consignment.GameMove = string.Join(';', Move);

            consignment.StatusID = 1;

            await StaticResouses.mainControler.ConsignmentControler.Update(consignment, consignment.ConsignmentID);

            IsGameContinues = false;

            TableResult tableResult = new() 
            {
                EventID = StaticResouses.mainControler.EventControler.nowEvent.EventID,
                PlayerID = consignment.whitePlayer.PlayerID,
                Result = (double)consignment.whitePlayer.Result,
                ConsignmentID = consignment.ConsignmentID
            };
            await StaticResouses.mainControler.ResultTableControler.InsertResult(StaticResouses.mainControler.EventControler.nowEvent.GetTableName(), tableResult);

            tableResult = new()
            {
                EventID = StaticResouses.mainControler.EventControler.nowEvent.EventID,
                PlayerID = consignment.blackPlayer.PlayerID,
                Result = (double)consignment.blackPlayer.Result,
                ConsignmentID = consignment.ConsignmentID
            };
            await StaticResouses.mainControler.ResultTableControler.InsertResult(StaticResouses.mainControler.EventControler.nowEvent.GetTableName(), tableResult);

            return;
        }

        private static double ELO(double mPlayer, double sPlayer, double result)
        {
            double d = sPlayer - mPlayer;
            int k;

            if (Math.Abs(d) > 400) d = 400;

            double Ea = 1 / (1 + Math.Pow(10, d / 400));

            if (mPlayer >= 2400) k = 10;
            else if (mPlayer >= 2000) k = 20;
            else k = 40;

            return Math.Round(mPlayer + k * (result - Ea), 1);
        }

        public async void DeleteLastMove(bool IsWhile)
        {
            MovePozition dataSet = await StaticResouses.mainControler.MoveTableControler.GetMovePozition(tableMove);
            string move = dataSet.Move.ToString();

            await StaticResouses.mainControler.MoveTableControler.DeleteLastMove(tableMove);

            UpdateFigureModel updateFigureModel = new()
            {
                Item1 = lastPozition.cell,
                Item2 = lastIDFigure.ToString()
            };

            await StaticResouses.mainControler.FigureTableControler.UpdatePozition(tableFigures,true, updateFigureModel);

            if (move[1] == 'x' || move[0] == 'x')
            {
                updateFigureModel = new()
                {
                    Item1 = orderCaptures.ToString()
                };
                await StaticResouses.mainControler.FigureTableControler.UpdateInGame(tableFigures, updateFigureModel);
            }

            int pozition = IsWhile ? 1 : 8;

            if (move == "O-O-O")
            {
                updateFigureModel = new()
                {
                    Item1 = pozition.ToString(),
                    Item2 = "0"
                };
                await StaticResouses.mainControler.FigureTableControler.UpdateCastlingLong(tableFigures, updateFigureModel);
            }

            if (move == "O-O")
            {
                updateFigureModel = new()
                {
                    Item1 = pozition.ToString(),
                    Item2 = "0"
                };
                await StaticResouses.mainControler.FigureTableControler.UpdateCastlingShort(tableFigures, updateFigureModel);
            }

            char temp = figure.FirstOrDefault(p => p == move[^1]);

            if (temp != default(char))
            {
                updateFigureModel = new()
                {
                    Item1 = lastPozition.cell,
                    Item2 = lastIDFigure.ToString()
                };
                await StaticResouses.mainControler.FigureTableControler.UpdatePozition(tableFigures,true, updateFigureModel);
            }

            Move.RemoveAt(Move.Count - 1);

            await GetFigures();
        }
    }
}