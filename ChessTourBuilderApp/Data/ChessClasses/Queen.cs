namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class Queen : Figure
    {
        private Rook rook;
        private Bishop bishop;

        public override string Name { get; } = "Q";


        public Queen(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID)
        {
            rook = new Rook(poziton, IsWhile, ID);
            bishop = new Bishop(poziton, IsWhile, ID);
        }

        public override string Move(Cell move, Figure[] figures)
        {
            if (rook.Move(move, figures) == null)
                if (bishop.Move(move, figures) == null) return null;
            return move.cell;
        }

        public override List<Cell> GetCells(Figure[] figures)
        {
            return (rook.GetCells(figures).Concat(bishop.GetCells(figures))).ToList();
        }
    }
}