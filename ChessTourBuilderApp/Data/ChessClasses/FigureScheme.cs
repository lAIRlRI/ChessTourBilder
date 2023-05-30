using System.Data;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class FigureScheme
    {
        public int ID { get; set; }
        public string Figure { get; set; }
        public string Pozition { get; set; }
        public bool IsWhile { get; set; }
        public bool InGame { get; set; } = true;
        public bool IsMoving { get; set; } = false;
        public int EatID { get; set; } = 0;
    }
}