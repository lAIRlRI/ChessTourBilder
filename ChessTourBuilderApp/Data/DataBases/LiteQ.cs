using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.DataBases
{
    internal class LiteQ : IDBQ
    {
        public string GetTableMove(string table) 
        {
            string str = table[1..^1];

            return $"CREATE TABLE \"{str}\" (" +
                "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                "PlayerID INTEGER NOT NULL," +
                "Move NVARCHAR(10) NOT NULL," +
                "Pozition NVARCHAR(10) NOT NULL," +
                "ConsignmentID INTEGER NOT NULL," +
                "TourID INTEGER NOT NULL," +
                "LastMove INTEGER NOT NULL DEFAULT 0," +
                "Winner INTEGER NOT NULL DEFAULT 0);";
        }

        public string GetTableFigures(string table) 
        {
            string str = table[1..^1];

            return $"CREATE TABLE \"{str}\" (" +
                "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                "Figure NVARCHAR(1) not null," +
                "Pozition nvarchar(2) not null," +
                "IsWhile INTEGER NOT NULL," +
                "InGame INTEGER NOT NULL DEFAULT 1," +
                "IsMoving INTEGER NOT NULL DEFAULT 0," +
                "EatID INTEGER NOT NULL DEFAULT 0);";
        }

        public string GetWinner(string table, int ID)
        {
            return $"UPDATE {table} SET Winner = 1 WHERE ID IN (SELECT ID FROM {table} WHERE PlayerID = {ID} ORDER BY ID DESC LIMIT 1);";
        }

        public string GetLastMove(string table)
        {
            return $"UPDATE {table} SET LastMove = 1 WHERE ID IN (SELECT ID FROM {table} ORDER BY ID DESC LIMIT 1);";
        }

        public string GetMovePozition(string table)
        {
            return $"select Move, Pozition from {table} where ID in (SELECT ID FROM {table} ORDER BY ID DESC LIMIT 1);";
        }

        public string DeleteTableMove(string table)
        {
            return $"Delete from {table} where ID in (SELECT ID FROM {table} ORDER BY ID DESC LIMIT 1)";
        }

        public string DeleteTableFigures(string table)
        {
            return $"Delete from {table} WHERE ID in (SELECT ID FROM {table} ORDER BY ID DESC LIMIT 1)";
        }

        public string GetResultСircle()
        {
            return $"WITH playerSum AS (SELECT PlayerID, SUM(Result) AS Points FROM {Controler.EventControler.nowEvent.GetTableName()} WHERE Result <> 0.5 GROUP BY PlayerID) " +
                "SELECT FirstName || ' ' || MiddleName AS Fi, Points FROM Player pl INNER JOIN playerSum p ON pl.FIDEID = p.PlayerID ORDER BY Points DESC;";
        }

        public string GetResult()
        {
            return $"WITH playerSum AS (SELECT PlayerID, SUM(Result) AS Points FROM {Controler.EventControler.nowEvent.GetTableName()} GROUP BY PlayerID) " +
                "SELECT FirstName || ' ' || MiddleName AS Fi, Points FROM Player pl INNER JOIN playerSum p ON pl.FIDEID = p.PlayerID ORDER BY Points DESC;";
        }
    }
}