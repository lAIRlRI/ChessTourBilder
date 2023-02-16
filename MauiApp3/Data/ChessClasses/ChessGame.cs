using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    internal class ChessGame
    {
        Figure[] figures = new Figure[32];

        public ChessGame()
        {
            CreateChessTable();
        }

        private Figure[] GetFigures() 
        {
            DataBase.Conn();
        }

        private void CreateChessTable() 
        {
            string str = "create table #Figures(" +
                "Figure nvarchar(1) not null," +
                "Pozition nvarchar(2) not null," +
                "PlayerID int not null," +
                "IsWhile bit not null," +
                "InGame bit not null default 1)" +
                "insert into #Figures (Figure,Pozition,PlayerID,IsWhile)" +
                "values ('','A2', 1111111, 1)," +
                "('','B2', 1111111, 1)," +
                "('','C2', 1111111, 1)," +
                "('','D2', 1111111, 1)," +
                "('','E2', 1111111, 1)," +
                "('','F2', 1111111, 1)," +
                "('','G2', 1111111, 1)," +
                "('','H2', 1111111, 1)," +
                "('','A7', 2222222, 0)," +
                "('','B7', 2222222, 0)," +
                "('','C7', 2222222, 0)," +
                "('','D7', 2222222, 0)," +
                "('','E7', 2222222, 0)," +
                "('','F7', 2222222, 0)," +
                "('','G7', 2222222, 0)," +
                "('','H7', 2222222, 0)," +
                "('K','E1', 1111111, 1)," +
                "('K','E8', 2222222, 0)," +
                "('Q','D1', 1111111, 1)," +
                "('Q','D8', 2222222, 0)," +
                "('B','C1', 1111111, 1)," +
                "('B','C8', 2222222, 0)," +
                "('B','F1', 1111111, 1)," +
                "('B','F8', 2222222, 0)," +
                "('N','G1', 1111111, 1)," +
                "('N','G8', 2222222, 0)," +
                "('N','B1', 1111111, 1)," +
                "('N','B8', 2222222, 0)," +
                "('R','H1', 1111111, 1)," +
                "('R','H8', 2222222, 0)," +
                "('R','A1', 1111111, 1)," +
                "('R','A8', 2222222, 0)";
            DataBase.ConnChange(str);
        }
    }
}
