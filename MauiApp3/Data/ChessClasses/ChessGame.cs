using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MauiApp3.Data.ChessClasses
{
    internal class ChessGame
    {
        public Figure[] Figures { get; set; } = new Figure[32];

        public List<string> Move { get; } = new List<string>();

        public ChessGame(int wPlayer, int bPlayer)
        {
            DataBaseFullConn.OpenConn();
            CreateChessTable(wPlayer, bPlayer);
            GetFigures();
        }

        private void GetFigures()
        {
            var table = DataBaseFullConn.ConnDataSet("select * from #Figures");
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
            string str = "UPDATE #Figures " +
                $"SET InGame = {InGame}" +
                $",Pozition = '{move}'" +
                $" WHERE Figure = '{figure.Name}' and Pozition = '{figure.Pozition}'";
            DataBaseFullConn.ConnChange(str);
            Move.Add(figure.Name + move);
            GetFigures();
        }

        public Figure[] GetFigure(bool IsWhile)
        {
            return Figures.Where(p => p.IsWhile == IsWhile).OrderBy(i => i.Name).ToArray();
        }

        private void CreateChessTable(int wPlayer, int bPlayer)
        {
            string str = "create table #Figures(" +
                "Figure nvarchar(1) not null," +
                "Pozition nvarchar(2) not null," +
                "PlayerID int not null," +
                "IsWhile bit not null," +
                "InGame bit not null default 1)" +
                "insert into #Figures (Figure,Pozition,PlayerID,IsWhile)" +
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

        public async void EndGame()
        {
            DataBaseFullConn.CloseCon();
        }
    }
}
