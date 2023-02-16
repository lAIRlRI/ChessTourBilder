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

        public Rook(string poziton, bool IsWhile):base(poziton, IsWhile){}

        public override void Move() { }
    }
}