using ChessTourBuilderApp.Data.HelpClasses;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class Cell
    {
        public int X;
        public int Y;
        public string cell;

        public Cell(string cell)
        {
            this.cell = cell;
            X = Convert.ToInt32(Helper.StringToInt[cell[0]]);
            Y = Convert.ToInt32(cell[1].ToString());
        }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            cell = GetString(x, y);
        }

        public static bool operator <(Cell x, Cell y) => x.X < y.X || x.Y < y.Y;

        public static bool operator >(Cell x, Cell y) => x.X > y.X || x.Y > y.Y;

        public static string GetString(int x, int y) => Helper.IntToString[x - 1] + y;
    }
}