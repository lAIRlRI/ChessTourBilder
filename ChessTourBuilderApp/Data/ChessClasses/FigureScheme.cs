using System.Data;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class FigureScheme
    {
        public static readonly Func<IDataReader, FigureScheme> mapper = r => new FigureScheme()
        {
            Name = r["Figure"].ToString(),
            Pozition = r["Pozition"].ToString(),
            IsWhile = Convert.ToBoolean(r["IsWhile"]),
            ID =  Convert.ToInt32(r["ID"]),
            InGame = Convert.ToBoolean(r["InGame"]),
            IsMoving = Convert.ToBoolean(r["IsMoving"])
        };

        public string Name { get; set; }
        public string Pozition { get; set; }
        public bool IsWhile { get; set; }
        public int ID { get; set; }
        public bool InGame { get; set; } = true;
        public bool IsMoving { get; set; } = false;
    }
}