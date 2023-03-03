using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Ладья
    ///</summary>
    internal class Rook : Figure
    {
        public override string Name { get; } = "R";
        public bool IsMoving { get; set; } = false;

        public Rook(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override bool Move(Cell pozition, Cell move)
        {
            return true;
        }
    }
}