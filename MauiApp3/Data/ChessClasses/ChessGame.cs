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
        public Figure[] Figures { get; } = new Figure[32];

        private string lastPozition;
        private string lastMove;
        private string lastFigure;

        public List<string> Move { get; } = new List<string>();

        private Consignment consignment;
        private string tableMove = "[" + EventControler.nowEvent.Name + DateTime.UtcNow + "]";
        private string tableFigures = "[#Figures" + DateTime.UtcNow + "]";

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
            CreateChessTable(consignment.whitePlayer.PlayerID, consignment.blackPlayer.PlayerID);

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
                        Figures[i] = new Pawn(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]));
                        break;
                    case "K":
                        Figures[i] = new King(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]));
                        break;
                    case "Q":
                        Figures[i] = new Queen(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]));
                        break;
                    case "B":
                        Figures[i] = new Bishop(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]));
                        break;
                    case "N":
                        Figures[i] = new Knight(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]));
                        break;
                    case "R":
                        Figures[i] = new Rook(item["Pozition"].ToString(), Convert.ToBoolean(item["IsWhile"]));
                        break;
                }
                i++;
            }
        }

        public void SetFigure(Figure figure, string move)
        {
            //figure.Move();
            int InGame = 1;
            string str = $"UPDATE {tableFigures} " +
                $"SET InGame = {InGame}" +
                $",Pozition = '{move}'" +
                $" WHERE Figure = '{figure.Name}' and Pozition = '{figure.Pozition}'";
            DataBaseFullConn.ConnChange(str);

            int ID = figure.IsWhile ? consignment.whitePlayer.PlayerID : consignment.blackPlayer.PlayerID;
            str = $"insert into {tableMove} (PlayerID,Move,ConsignmentID,TourID)" +
                        $" values ({ID},'{move}',{consignment.ConsignmentID},{consignment.TourID})";
            DataBaseFullConn.ConnChange(str);
            lastPozition = figure.Pozition;
            lastMove = move;
            lastFigure = figure.Name;
            Move.Add(figure.Name + move);
            GetFigures();
        }

        public Figure[] GetFigure(bool IsWhile)
        {
            return Figures.Where(p => p.IsWhile == IsWhile).OrderBy(i => i.Name).ToArray();
        }

        private void CreateChessTable(int wPlayer, int bPlayer)
        {

            string str = $"create table {tableFigures}(" +
                "Figure nvarchar(1) not null," +
                "Pozition nvarchar(2) not null," +
                "PlayerID int not null," +
                "IsWhile bit not null," +
                "InGame bit not null default 1)" +
                $"insert into {tableFigures} (Figure,Pozition,PlayerID,IsWhile)" +
                $"values ('','A2', {wPlayer}, 1)," +
                $"('','B2', {wPlayer}, 1)," +
                $"('','C2', {wPlayer}, 1)," +
                $"('','D2', {wPlayer}, 1)," +
                $"('','E2', {wPlayer}, 1)," +
                $"('','F2', {wPlayer}, 1)," +
                $"('','G2', {wPlayer}, 1)," +
                $"('','H2', {wPlayer}, 1)," +
                $"('','A7', {bPlayer}, 0)," +
                $"('','B7', {bPlayer}, 0)," +
                $"('','C7', {bPlayer}, 0)," +
                $"('','D7', {bPlayer}, 0)," +
                $"('','E7', {bPlayer}, 0)," +
                $"('','F7', {bPlayer}, 0)," +
                $"('','G7', {bPlayer}, 0)," +
                $"('','H7', {bPlayer}, 0)," +
                $"('K','E1', {wPlayer}, 1)," +
                $"('K','E8', {bPlayer}, 0)," +
                $"('Q','D1', {wPlayer}, 1)," +
                $"('Q','D8', {bPlayer}, 0)," +
                $"('B','C1', {wPlayer}, 1)," +
                $"('B','C8', {bPlayer}, 0)," +
                $"('B','F1', {wPlayer}, 1)," +
                $"('B','F8', {bPlayer}, 0)," +
                $"('N','G1', {wPlayer}, 1)," +
                $"('N','G8', {bPlayer}, 0)," +
                $"('N','B1', {wPlayer}, 1)," +
                $"('N','B8', {bPlayer}, 0)," +
                $"('R','H1', {wPlayer}, 1)," +
                $"('R','H8', {bPlayer}, 0)," +
                $"('R','A1', {wPlayer}, 1)," +
                $"('R','A8', {bPlayer}, 0)";
            DataBaseFullConn.ConnChange(str);
        }

        public void EndGame(double? result)
        {
            string str = $"update {tableMove} set " +
                $"LastMove = 1 " +
                $"where ID in (select top 1 ID from {tableMove} order by ID desc)";
            DataBaseFullConn.ConnChange(str);

            if (result == 0.5)
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
            DataBaseFullConn.ConnChange($"Delete from {tableMove} where ID in " +
                                           $"(select top 1 ID from {tableMove} order by ID desc)");
            string str = $"UPDATE {tableFigures} " +
               $"SET InGame = 1" +
               $",Pozition = '{lastPozition}'" +
               $" WHERE Figure = '{lastFigure}' and Pozition = '{lastMove}'";
            DataBaseFullConn.ConnChange(str);
            Move.RemoveAt(Move.Count - 1);
            GetFigures();
        }
    }
}