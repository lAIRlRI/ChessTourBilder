using ChessTourBuilderApp.Data.ChessClasses;
using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class FigureTableControlerLite : IFigureTableControler
    {
        public static readonly Func<IDataReader, FigureScheme> mapper = r => new FigureScheme()
        {
            Figure = r["Figure"].ToString(),
            Pozition = r["Pozition"].ToString(),
            IsWhile = Convert.ToBoolean(r["IsWhile"]),
            ID = Convert.ToInt32(r["ID"]),
            InGame = Convert.ToBoolean(r["InGame"]),
            IsMoving = Convert.ToBoolean(r["IsMoving"])
        };

        public async Task<bool> CreateFigureMove(string table)
        {
            await Task.Delay(2);
            string messege = $"CREATE TABLE {table} (" +
                "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                "Figure NVARCHAR(1) not null," +
                "Pozition nvarchar(2) not null," +
                "IsWhile INTEGER NOT NULL," +
                "InGame INTEGER NOT NULL DEFAULT 1," +
                "IsMoving INTEGER NOT NULL DEFAULT 0," +
                "EatID INTEGER NOT NULL DEFAULT 0);" +
                "" +
                $"INSERT INTO {table} (Figure,Pozition,IsWhile) " +
                "values ('','A2', 1),('','B2', 1),('','C2', 1),('','D2', 1),('','E2', 1),('','F2', 1),('','G2', 1),('','H2', 1)," +
                "('','A7', 0),('','B7', 0),('','C7', 0),('','D7', 0),('','E7', 0),('','F7', 0),('','G7', 0),('','H7', 0),('K','E1', 1)," +
                "('K','E8', 0),('Q','D1', 1),('Q','D8', 0),('B','C1', 1),('B','C8', 0),('B','F1', 1),('B','F8', 0),('N','G1', 1),('N','G8', 0)," +
                "('N','B1', 1),('N','B8', 0),('R','H1', 1),('R','H8', 0),('R','A1', 1),('R','A8', 0);";

            DataBase.Execute(messege);
            return true;
        }

        public async Task<bool> InsertFigure(string table, FigureScheme value)
        {
            await Task.Delay(2);
            string formattable = $"insert into {table} (Figure,Pozition,IsWhile)" +
                $"values ('{value.Figure}','{value.Pozition}',{value.IsWhile})";
            DataBase.Execute(formattable);
            return true;
        }

        public async Task<List<FigureScheme>> GetAll(string table)
        {
            await Task.Delay(2);
            return DataBase.ReadFull($"select * from {table}", mapper);
        }

        public async Task<bool> UpdatePozition(string table, bool IsMoving, UpdateFigureModel view)
        {
            await Task.Delay(2);

            int ism = IsMoving ? 1 : 0;
            string formattable = $"UPDATE {table} " +
               $"SET Pozition = '{view.Item1}'," +
               $"IsMoving = {ism}" +
               $" WHERE ID = {view.Item2}";

            DataBase.Execute(formattable);
            return true;
        }

        public async Task<bool> UpdateInGame(string table, UpdateFigureModel view)
        {
            await Task.Delay(2);

            string formattable = $"UPDATE {table} " +
               $"SET InGame = 1" +
               $" WHERE EatID = {view.Item1}";

            DataBase.Execute(formattable);
            return true;
        }

        public async Task<bool> UpdateEat(string table, UpdateFigureModel view)
        {
            await Task.Delay(2);

            string formattable = $"UPDATE {table} " +
               $"SET InGame = 0," +
               $" EatID = {view.Item1}" +
               $" WHERE ID = {view.Item2}";

            DataBase.Execute(formattable);
            return true;
        }

        public async Task<bool> UpdateCastlingLong(string table, UpdateFigureModel view)
        {
            await Task.Delay(2);

            string formattable = $"UPDATE {table} " +
                     $"SET Pozition = 'E{view.Item1}'," +
                     $"IsMoving = {view.Item2}" +
                     $" WHERE Pozition = 'C{view.Item1}';" +

                     $"UPDATE {table} " +
                     $"SET Pozition = 'A{view.Item1}'," +
                     $"IsMoving = {view.Item2}" +
                     $" WHERE Pozition = 'D{view.Item1}'";

            DataBase.Execute(formattable);
            return true;
        }

        public async Task<bool> UpdateCastlingShort(string table, UpdateFigureModel view)
        {
            await Task.Delay(2);

            string formattable = $"UPDATE {table} " +
                    $"SET Pozition = 'E{view.Item1}'," +
                    $"IsMoving = {view.Item2}" +
                    $" WHERE Pozition = 'G{view.Item1}';" +

                    $"UPDATE {table} " +
                    $"SET Pozition = 'H{view.Item1}'," +
                    $"IsMoving = {view.Item2}" +
                    $" WHERE Pozition = 'F{view.Item1}'";

            DataBase.Execute(formattable);
            return true;
        }
    }
}