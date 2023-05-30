using ChessTourBuilderApp.Data.ChessClasses;
using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class MoveTableControlerLite : IMoveTableControler
    {
        public static readonly Func<IDataReader, MovePozition> mapper = r => new MovePozition()
        {
            Move = r["Move"].ToString(),
            Pozition = r["Pozition"].ToString()
        };

        public async Task<bool> CreateTableMove(string table)
        {
            await Task.Delay(2);
            string messege = $"CREATE TABLE {table} (" +
                "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                "PlayerID INTEGER NOT NULL," +
                "Move NVARCHAR(10) NOT NULL," +
                "Pozition NVARCHAR(10) NOT NULL," +
                "ConsignmentID INTEGER NOT NULL," +
                "TourID INTEGER NOT NULL," +
                "LastMove INTEGER NOT NULL DEFAULT 0," +
                "Winner INTEGER NOT NULL DEFAULT 0);";

            DataBase.Execute(messege);
            return true;
        }

        public async Task<bool> PutWinner(string table, int ID)
        {
            await Task.Delay(2);
            string formattable = $"UPDATE {table} SET Winner = 1 WHERE ID IN (SELECT ID FROM {table} WHERE PlayerID = {ID} ORDER BY ID DESC LIMIT 1);";
            DataBase.Execute(formattable);
            return true;
        }

        public async Task<bool> PutLastMove(string table)
        {
            await Task.Delay(2);
            string formattable = $"UPDATE {table} SET LastMove = 1 WHERE ID IN (SELECT ID FROM {table} ORDER BY ID DESC LIMIT 1);";
            DataBase.Execute(formattable);
            return true;
        }

        public async Task<bool> PostMove(string table, MoveTableModel value)
        {
            await Task.Delay(2);
            string formattable = $"insert into {table} (PlayerID,Move,ConsignmentID,TourID, Pozition, FigureID)" +
                         $" values ({value.PlayerID},'{value.Move}',{value.ConsignmentID},{value.TourID},'{value.Pozition}', {value.ID})";
            DataBase.Execute(formattable);
            return true;
        }

        public async Task<bool> DeleteLastMove(string table)
        {
            await Task.Delay(2);
            string formattable = $"Delete from {table} where ID in (SELECT ID FROM {table} ORDER BY ID DESC LIMIT 1)";
            DataBase.Execute(formattable);
            return true;
        }

        public async Task<MovePozition> GetMovePozition(string table) 
        {
            await Task.Delay(2);
            MovePozition models = DataBase.Read($"select Move, Pozition from {table} where ID in (SELECT ID FROM {table} ORDER BY ID DESC LIMIT 1);", mapper)[0];
            return models;
        }
    }
}