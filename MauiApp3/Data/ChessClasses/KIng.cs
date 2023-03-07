using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Король
    ///</summary>
    internal class King : Figure
    {
        public override string Name { get;} = "K";
        public bool IsMoving { get; set; } = false;
        public King(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override bool Move(Cell pozition, Figure[] figures, Cell move)
        {
            if (Math.Abs(pozition.X - move.X) > 1 || Math.Abs(pozition.Y - move.Y) > 1) return false;
            return true;
        }
    }
}