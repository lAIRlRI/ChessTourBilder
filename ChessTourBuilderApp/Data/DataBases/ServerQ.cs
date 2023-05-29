using ChessTourBuilderApp.Data.HelpClasses;

namespace ChessTourBuilderApp.Data.DataBases
{
    internal class ServerQ : IDBQ
    {
        public string GetTableMove(string table)
        {
            return $"create table {table} (" +
                "ID int identity(1,1) not null," +
                "PlayerID int not null," +
                "Move nvarchar(10) not null," +
                "Pozition nvarchar(10) not null," +
                "ConsignmentID int not null," +
                "TourID int not null," +
                "LastMove bit not null default 0," +
                "Winner bit not null default 0);";
        }

        public string GetTableFigures(string table)
        {
            return $"create table {table}(" +
                "ID int identity(1,1) not null," +
                "Figure nvarchar(1) not null," +
                "Pozition nvarchar(2) not null," +
                "IsWhile bit not null," +
                "InGame bit not null default 1," +
                "IsMoving bit not null default 0," +
                "EatID int not null default 0)";
        }

        public string GetWinner(string table, int ID)
        {
            return $"update { table} set " 
                + $"Winner = 1 " +
                    $"where ID in (select top 1 ID from {table} where PlayerID = {ID} order by ID desc)";
        }

        public string GetLastMove(string table)
        {
            return $"update {table} set " +
                $"LastMove = 1 " +
                $"where ID in (select top 1 ID from {table} order by ID desc)";
        }

        public string GetMovePozition(string table)
        {
            return $"select Move, Pozition from {table} " +
                "where ID in " +
                $"(select top 1 ID from {table} order by ID desc)";
        }

        public string DeleteTableMove(string table)
        {
            return $"Delete from {table} where ID in " +
                                           $"(select top 1 ID from {table} order by ID desc)";
        }

        public string DeleteTableFigures(string table)
        {
            return $"Delete from {table} " +
                     $" WHERE ID in (select top 1 ID from {table} order by ID desc)";
        }

        public string GetResult()
        {
            return ";with playerSum as" +
            $"(select PlayerID, Sum(Result) as Points from {StaticResouses.mainControler.EventControler.nowEvent.GetTableName()} group by PlayerID)" +
            "select Concat(FirstName, ' ', MiddleName) as Fi, Points from Player pl inner join playerSum p on pl.FIDEID = p.PlayerID order by Points desc";
        }

        public string GetResultСircle()
        {
            return ";with playerSum as" +
            $"(select PlayerID, Sum(Result) as Points from {StaticResouses.mainControler.EventControler.nowEvent.GetTableName()} where Result <> 0.5 group by PlayerID)" +
            "select ROW_NUMBER() OVER (ORDER BY Points DESC) AS Pozition, Concat(FirstName, ' ', MiddleName) as Fi, Points from Player pl inner join playerSum p on pl.FIDEID = p.PlayerID order by Points desc";
        }

        public bool UpdateStatus()
        {
            DataBase.Execute("UPDATE Event SET StatusID = CASE " +
                "WHEN DataStart > GETDATE() THEN 2 " +
                "WHEN DataFinish >= GETDATE() AND DataStart <= GETDATE() THEN 3 " +
                "ELSE 1 END;");
            return true;
        }
    }
}