using ChessTourBuilderApp.Data.DataBases;
using ChessTourBuilderApp.Data.Model;
using System.Data;

namespace ChessTourBuilderApp.Data.Controler.ControlerServer
{
    internal class ResultTableControlerLite : IResultTableControler
    {
        public static readonly Func<IDataReader, ResultSheme> mapper = r => new ResultSheme()
        {
            Points = r["Points"].ToString(),
            Fi = r["Fi"].ToString(),
            Pozition = Convert.ToInt32(r["Pozition"])
        };

        public async Task<bool> CreateResultTable(string table)
        {
            await Task.Delay(2);
            DataBase.Execute($"create table {table} (" +
                                                "EventID int not null," +
                                                "PlayerID int not null," +
                                                "Result float not null," +
                                                "ConsignmentID int not null)");
            return true;
        }

        public async Task<bool> InsertResult(string table, TableResult value)
        {
            await Task.Delay(2);
            string formattable = $"insert into {table} (EventID,PlayerID,Result,ConsignmentID)" +
                    $"Values ({value.EventID},{value.PlayerID},{value.Result},{value.ConsignmentID})";
            DataBase.Execute(formattable);
            return true;
        }

        public async Task<List<ResultSheme>> GetResultTable(string table) 
        {
            await Task.Delay(2);
            string formattable = $"WITH playerSum AS (SELECT PlayerID, SUM(Result) AS Points FROM {table} WHERE Result <> 0.5 GROUP BY PlayerID) " +
                "SELECT ROW_NUMBER() OVER (ORDER BY Points DESC) AS Pozition,FirstName || ' ' || MiddleName AS Fi,Points FROM Player AS pl " +
                "INNER JOIN playerSum AS p ON pl.FIDEID = p.PlayerID ORDER BY Points DESC;";
            return DataBase.Read(formattable, mapper);
        }

        public async Task<List<ResultSheme>> GetResultTableСircle(string table) 
        {
            await Task.Delay(2);
            string formattable = $"WITH playerSum AS (SELECT PlayerID, SUM(Result) AS Points FROM {table} GROUP BY PlayerID) " +
                "SELECT ROW_NUMBER() OVER (ORDER BY Points DESC) AS Pozition,FirstName || ' ' || MiddleName AS Fi,Points FROM Player AS pl " +
                "INNER JOIN playerSum AS p ON pl.FIDEID = p.PlayerID ORDER BY Points DESC;";
            return DataBase.Read(formattable, mapper);
        }
    }
}