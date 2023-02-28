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
        public override string Name { get; } = "K";

        public King(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override void Move() { }
    }
}