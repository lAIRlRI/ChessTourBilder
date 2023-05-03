using ChessTourBuilderApp.Data.Controler;
using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.HelpClasses;
using ChessTourBuilderApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class ChessGame
    {
        private Consignment consignment;
        private readonly string tableMove = "[" + EventControler.nowEvent.Name + DateTime.UtcNow + "]";
        private readonly string tableFigures = "[#Figures" + DateTime.UtcNow + "]";
        private Cell lastPozition;
        private int orderCaptures = 0;
        private int lastIDFigure;

        public Figure[] Figures { get; private set; } = new Figure[32];
        public List<string> Move { get; } = new List<string>();
        public bool IsGameContinues { get; private set; } = true;

        public ChessGame(Consignment consignment)
        {
            DataBase.OpenConn();
            this.consignment = consignment;
            DataBase.ExecuteFull(StaticResouses.dBQ.GetTableMove(tableMove));

            DataBase.ExecuteFull($"update Consignment set TableName = '{tableMove}' where ConsignmentID = {consignment.ConsignmentID}");
            CreateChessTable();

            GetFigures();
        }

        private void GetFigures()
        {
            var table = DataBase.ReadFull($"select * from {tableFigures}", FigureScheme.mapper);
            int i = 0;
            Figures = new Figure[table.Count];

            foreach (var item in table)
            {
                //хештаблицы???; избавиться от object
                switch (item.Name)
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

        public string SetFigure(Figure figure, string move)
        {
            if (figure == null) return "no";

            (string, int) function = figure.SetFigureTrueMove(Figures, move, tableFigures, orderCaptures, tableMove);

            string insertMove = function.Item1;
            orderCaptures = function.Item2;

            if (insertMove == null) return "no";
            if ("rpt" == insertMove) return "rpt";

            string str;

            int ID = figure.IsWhile ? consignment.whitePlayer.PlayerID : consignment.blackPlayer.PlayerID;
            str = $"insert into {tableMove} (PlayerID,Move,ConsignmentID,TourID, Pozition)" +
                        $" values ({ID},'{insertMove}',{consignment.ConsignmentID},{consignment.TourID},'{figure.Pozition.cell}')";
            DataBase.ExecuteFull(str);

            lastPozition = figure.Pozition;
            lastIDFigure = figure.ID;

            Move.Add(insertMove);

            GetFigures();

            return "ok";
        }

        public bool InsertFigure(string pozition, string name, bool IsWhile)
        {
            int b = IsWhile ? 1 : 0;
            string str = $"insert into {tableFigures} (Figure,Pozition,IsWhile)" +
                $"values ('{name}','{pozition}',{b})";
            DataBase.ExecuteFull(str);
            int ID = IsWhile ? consignment.whitePlayer.PlayerID : consignment.blackPlayer.PlayerID;
            str = $"insert into {tableMove} (PlayerID,Move,ConsignmentID,TourID)" +
                        $" values ({ID},'{pozition + name}',{consignment.ConsignmentID},{consignment.TourID})";
            DataBase.ExecuteFull(str);
            GetFigures();
            Move.Add(pozition + name);
            return true;
        }

        public Figure[] GetFigure(bool IsWhile) => Figures.Where(p => p.IsWhile == IsWhile && p.InGame == true).ToArray();

        private void CreateChessTable()
        {
            string str = StaticResouses.dBQ.GetTableFigures(tableFigures);
            DataBase.ExecuteFull(str);
            str = $"INSERT INTO {tableFigures} (Figure,Pozition,IsWhile)" +
                "values ('','A2', 1)," +
                "('','B2', 1)," +
                "('','C2', 1)," +
                "('','D2', 1)," +
                "('','E2', 1)," +
                "('','F2', 1)," +
                "('','G2', 1)," +
                "('','H2', 1)," +
                "('','A7', 0)," +
                "('','B7', 0)," +
                "('','C7', 0)," +
                "('','D7', 0)," +
                "('','E7', 0)," +
                "('','F7', 0)," +
                "('','G7', 0)," +
                "('','H7', 0)," +
                "('K','E1', 1)," +
                "('K','E8', 0)," +
                "('Q','D1', 1)," +
                "('Q','D8', 0)," +
                "('B','C1', 1)," +
                "('B','C8', 0)," +
                "('B','F1', 1)," +
                "('B','F8', 0)," +
                "('N','G1', 1)," +
                "('N','G8', 0)," +
                "('N','B1', 1)," +
                "('N','B8', 0)," +
                "('R','H1', 1)," +
                "('R','H8', 0)," +
                "('R','A1', 1)," +
                "('R','A8', 0)";
            DataBase.ExecuteFull(str);
        }

        public void EndGame(double? result)
        {
            string str = StaticResouses.dBQ.GetLastMove(tableMove);
            DataBase.ExecuteFull(str);

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

                str = StaticResouses.dBQ.GetWinner(tableMove, ID);

            }
            DataBase.ExecuteFull(str);
            DataBase.CloseCon();

            consignment.whitePlayer.player.ELORating = ELO((double)consignment.whitePlayer.player.ELORating, (double)consignment.blackPlayer.player.ELORating, (double)consignment.whitePlayer.Result);
            consignment.blackPlayer.player.ELORating = ELO((double)consignment.blackPlayer.player.ELORating, (double)consignment.whitePlayer.player.ELORating, (double)consignment.blackPlayer.Result);

            consignment.GameMove = string.Join(';', Move);

            consignment.StatusID = 1;

            ConsignmentControler.Update(consignment);

            IsGameContinues = false;

            if (result != 2)
            {
                DataBase.ExecuteFull($"insert into {EventControler.nowEvent.GetTableName()} (EventID,PlayerID,Result,ConsignmentID)" +
                   $"Values ({EventControler.nowEvent.EventID},{consignment.whitePlayer.PlayerID},{consignment.whitePlayer.Result},{consignment.ConsignmentID})");
                DataBase.ExecuteFull($"insert into {EventControler.nowEvent.GetTableName()} (EventID,PlayerID,Result,ConsignmentID)" +
                    $"Values ({EventControler.nowEvent.EventID},{consignment.blackPlayer.PlayerID},{consignment.blackPlayer.Result},{consignment.ConsignmentID})");
                return;
            }

            DataBase.ExecuteFull($"insert into {EventControler.nowEvent.GetTableName()} (EventID,PlayerID,Result,ConsignmentID)" +
                $"Values ({EventControler.nowEvent.EventID},{consignment.whitePlayer.PlayerID},0.5,{consignment.ConsignmentID})");
            DataBase.ExecuteFull($"insert into {EventControler.nowEvent.GetTableName()} (EventID,PlayerID,Result,ConsignmentID)" +
                $"Values ({EventControler.nowEvent.EventID},{consignment.blackPlayer.PlayerID},0.5,{consignment.ConsignmentID})");


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

        public void DeleteLastMove(bool IsWhile)
        {
            List<TableFiguresScheme> dataSet = DataBase.ReadFull(StaticResouses.dBQ.GetMovePozition(tableMove), TableFiguresScheme.mapper);
            string move = dataSet[0].Move.ToString();

            DataBase.ExecuteFull(StaticResouses.dBQ.DeleteTableMove(tableMove));

            string str = $"UPDATE {tableFigures} " +
               $"SET Pozition = '{lastPozition.cell}'" +
               $" WHERE ID = {lastIDFigure}";
            DataBase.ExecuteFull(str);

            if (move[1] == 'x' || move[0] == 'x')
            {
                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 1" +
                $" WHERE EatID = {orderCaptures}";
                DataBase.ExecuteFull(str);
            }

            int pozition = IsWhile ? 1 : 8;

            if (move == "O-O-O")
            {
                str = $"UPDATE {tableFigures} " +
                     $"SET Pozition = 'E{pozition}'," +
                     $"IsMoving = 0" +
                     $" WHERE Pozition = 'C{pozition}'";
                DataBase.ExecuteFull(str);

                str = $"UPDATE {tableFigures} " +
                     $"SET Pozition = 'A{pozition}'," +
                     $"IsMoving = 0" +
                     $" WHERE Pozition = 'D{pozition}'";
                DataBase.ExecuteFull(str);
            }

            if (move == "O-O")
            {
                str = $"UPDATE {tableFigures} " +
                     $"SET Pozition = 'E{pozition}'," +
                     $"IsMoving = 0" +
                     $" WHERE Pozition = 'G{pozition}'";
                DataBase.ExecuteFull(str);

                str = $"UPDATE {tableFigures} " +
                     $"SET Pozition = 'H{pozition}'," +
                     $"IsMoving = 0" +
                     $" WHERE Pozition = 'F{pozition}'";
                DataBase.ExecuteFull(str);
            }

            char[] figure = new char[] { 'Q', 'N', 'B', 'R' };

            char temp = figure.FirstOrDefault(p => p == move[^1]);

            if (temp != default(char))
            {
                str = $"UPDATE {tableFigures} " +
               $"SET Pozition = '{lastPozition.cell}'" +
               $" WHERE ID = {lastIDFigure}";
                DataBase.ExecuteFull(str);

                str = StaticResouses.dBQ.GetTableFigures(tableFigures);
                DataBase.ExecuteFull(str);
            }

            Move.RemoveAt(Move.Count - 1);

            GetFigures();
        }
    }
}