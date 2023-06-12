using System.Data;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class MovePozition
    {
        public string Move { get; set; }
        public string Pozition { get; set; }

        public MovePozition(string move, string pozition)
        {
            Move = move;
            Pozition = pozition;
        }

        public MovePozition()
        {

        }
    }
}