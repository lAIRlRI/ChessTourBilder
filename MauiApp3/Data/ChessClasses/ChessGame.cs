using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MauiApp3.Data.Model;
using MauiApp3.Data.Controler;

namespace MauiApp3.Data.ChessClasses
{
    internal class ChessGame
    {
        private Consignment consignment;
        private string tableMove = "[" + EventControler.nowEvent.Name + DateTime.UtcNow + "]";
        private string tableFigures = "[#Figures" + DateTime.UtcNow + "]";
        private Figure NotGameFigure;
        private string lastPozition;
        private int lastFigure = 0;
        private int lastIDFigure;

        public Figure[] Figures { get; } = new Figure[32];
        public List<string> Move { get; } = new List<string>();

        public ChessGame(Consignment consignment)
        {
            DataBaseFullConn.OpenConn();
            this.consignment = consignment;
            DataBaseFullConn.ConnChange($"create table {tableMove} (" +
                "ID int identity(1,1) not null," +
                "PlayerID int not null," +
                "Move nvarchar(10) not null," +
                "ConsignmentID int not null," +
                "TourID int not null," +
                "LastMove bit not null default 0," +
                "Winner bit not null default 0," +
                "FOREIGN KEY(PlayerID) REFERENCES Player(FIDEID)," +
                "FOREIGN KEY(ConsignmentID) REFERENCES Consignment(ConsignmentID)," +
                "FOREIGN KEY(TourID) REFERENCES Tour(TourID))");

            DataBaseFullConn.ConnChange($"update Consignment set TableName = '{tableMove}' where ConsignmentID = {consignment.ConsignmentID}");
            CreateChessTable();

            GetFigures();
        }

        private void GetFigures()
        {
            var table = DataBaseFullConn.ConnDataSet($"select * from {tableFigures}");
            int i = 0;
            foreach (DataRow item in table.Tables[0].Rows)
            {
                //хештаблицы???; избавиться от object
                switch (item["Figure"])
                {
                    case "":
                        Figures[i] = new Pawn(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]), Convert.ToInt32(item["ID"]))
                        { InGame = Convert.ToBoolean(item["InGame"]) };
                        break;
                    case "K":
                        Figures[i] = new King(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]), Convert.ToInt32(item["ID"]))
                        { InGame = Convert.ToBoolean(item["InGame"]) };
                        break;
                    case "Q":
                        Figures[i] = new Queen(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]), Convert.ToInt32(item["ID"]))
                        { InGame = Convert.ToBoolean(item["InGame"]) };
                        break;
                    case "B":
                        Figures[i] = new Bishop(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]), Convert.ToInt32(item["ID"]))
                        { InGame = Convert.ToBoolean(item["InGame"]) };
                        break;
                    case "N":
                        Figures[i] = new Knight(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]), Convert.ToInt32(item["ID"]))
                        { InGame = Convert.ToBoolean(item["InGame"]) };
                        break;
                    case "R":
                        Figures[i] = new Rook(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]), Convert.ToInt32(item["ID"]))
                        { InGame = Convert.ToBoolean(item["InGame"]) };
                        break;
                }
                i++;
            }
        }

        public bool SetFigure(Figure figure, string move)
        {
            if (figure == null) return false;
            string insertMove = figure.Name + move;
            NotGameFigure = Figures.Where(p => p.Pozition == move && p.InGame == true).FirstOrDefault();
            string str;
            if (NotGameFigure != default(Figure))
            {
                if (NotGameFigure.IsWhile == figure.IsWhile) return false;
                lastFigure++;
                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 0," +
                $" EatID = {lastFigure}" +
                $" WHERE ID = {NotGameFigure.ID}";
                DataBaseFullConn.ConnChange(str);
                insertMove = figure.Name + "x" + move;
            }
            str = $"UPDATE {tableFigures} " +
                $"SET Pozition = '{move}'" +
                $" WHERE ID = {figure.ID}";
            DataBaseFullConn.ConnChange(str);

            int ID = figure.IsWhile ? consignment.whitePlayer.PlayerID : consignment.blackPlayer.PlayerID;
            str = $"insert into {tableMove} (PlayerID,Move,ConsignmentID,TourID)" +
                        $" values ({ID},'{insertMove}',{consignment.ConsignmentID},{consignment.TourID})";
            DataBaseFullConn.ConnChange(str);
            lastPozition = figure.Pozition;
            lastIDFigure = figure.ID;
            Move.Add(insertMove);
            GetFigures();
            return true;
        }

        public Figure[] GetFigure(bool IsWhile)
        {
            return Figures.Where(p => p.IsWhile == IsWhile && p.InGame == true).OrderBy(i => i.Name).ToArray();
        }

        private void CreateChessTable()
        {

            string str = $"create table {tableFigures}(" +
                "ID int identity(1,1) not null," +
                "Figure nvarchar(1) not null," +
                "Pozition nvarchar(2) not null," +
                "IsWhile bit not null," +
                "InGame bit not null default 1," +
                "EatID int not null default 0)" +
                $"insert into {tableFigures} (Figure,Pozition,IsWhile)" +
                $"values ('','A2', 1)," +
                $"('','B2', 1)," +
                $"('','C2', 1)," +
                $"('','D2', 1)," +
                $"('','E2', 1)," +
                $"('','F2', 1)," +
                $"('','G2', 1)," +
                $"('','H2', 1)," +
                $"('','A7', 0)," +
                $"('','B7', 0)," +
                $"('','C7', 0)," +
                $"('','D7', 0)," +
                $"('','E7', 0)," +
                $"('','F7', 0)," +
                $"('','G7', 0)," +
                $"('','H7', 0)," +
                $"('K','E1', 1)," +
                $"('K','E8', 0)," +
                $"('Q','D1', 1)," +
                $"('Q','D8', 0)," +
                $"('B','C1', 1)," +
                $"('B','C8', 0)," +
                $"('B','F1', 1)," +
                $"('B','F8', 0)," +
                $"('N','G1', 1)," +
                $"('N','G8', 0)," +
                $"('N','B1', 1)," +
                $"('N','B8', 0)," +
                $"('R','H1', 1)," +
                $"('R','H8', 0)," +
                $"('R','A1', 1)," +
                $"('R','A8', 0)";
            DataBaseFullConn.ConnChange(str);
        }

        public void EndGame(double? result)
        {
            string str = $"update {tableMove} set " +
                $"LastMove = 1 " +
                $"where ID in (select top 1 ID from {tableMove} order by ID desc)";
            DataBaseFullConn.ConnChange(str);

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

                str = $" update {tableMove} set " +
                    $"Winner = 1 " +
                    $"where ID in (select top 1 ID from {tableMove} where PlayerID = {ID} order by ID desc)";

            }
            DataBaseFullConn.ConnChange(str);
            consignment.whitePlayer.player.ELORating = ELO((double)consignment.whitePlayer.player.ELORating, (double)consignment.blackPlayer.player.ELORating, (double)consignment.whitePlayer.Result);
            consignment.blackPlayer.player.ELORating = ELO((double)consignment.blackPlayer.player.ELORating, (double)consignment.whitePlayer.player.ELORating, (double)consignment.blackPlayer.Result);
            consignment.GameMove = string.Join(';', Move);
            ConsignmentControler.Update(consignment);
            if (result != 2) 
            {
                DataBaseFullConn.ConnChange($"insert into {EventControler.nowEvent.GetTableName()} (EventID,PlayerID,Result,ConsignmentID)" +
                   $"Values ({EventControler.nowEvent.EventID},{consignment.whitePlayer.PlayerID},{consignment.whitePlayer.Result},{consignment.ConsignmentID})");
                DataBaseFullConn.ConnChange($"insert into {EventControler.nowEvent.GetTableName()} (EventID,PlayerID,Result,ConsignmentID)" +
                    $"Values ({EventControler.nowEvent.EventID},{consignment.blackPlayer.PlayerID},{consignment.blackPlayer.Result},{consignment.ConsignmentID})");
                return;
            }
            DataBaseFullConn.ConnChange($"insert into {EventControler.nowEvent.GetTableName()} (EventID,PlayerID,Result,ConsignmentID)" +
                $"Values ({EventControler.nowEvent.EventID},{consignment.whitePlayer.PlayerID},0.5,{consignment.ConsignmentID})");
            DataBaseFullConn.ConnChange($"insert into {EventControler.nowEvent.GetTableName()} (EventID,PlayerID,Result,ConsignmentID)" +
                $"Values ({EventControler.nowEvent.EventID},{consignment.blackPlayer.PlayerID},0.5,{consignment.ConsignmentID})");
        }

        private double ELO(double mPlayer, double sPlayer, double result)
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

        public void DeleteLastMove()
        {

            DataSet dataSet = DataBaseFullConn.ConnDataSet($"select Move from {tableMove} " +
                "where ID in " +
                $"(select top 1 ID from {tableMove} order by ID desc)");
            string move = dataSet.Tables[0].Rows[0][0].ToString();

            DataBaseFullConn.ConnChange($"Delete from {tableMove} where ID in " +
                                           $"(select top 1 ID from {tableMove} order by ID desc)");

            string str = $"UPDATE {tableFigures} " +
               $"SET Pozition = '{lastPozition}'" +
               $" WHERE ID = {lastIDFigure}";
            DataBaseFullConn.ConnChange(str);

            if (move[1] == 'x' || move[0] == 'x')
            {
                str = $"UPDATE {tableFigures} " +
                $"SET InGame = 1" +
                $" WHERE EatID = {lastFigure}";
                DataBaseFullConn.ConnChange(str);
            }
            Move.RemoveAt(Move.Count - 1);
            GetFigures();
        }
    }
}