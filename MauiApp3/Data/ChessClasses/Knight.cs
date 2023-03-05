using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Конь
    ///</summary>
    internal class Knight : Figure
    {
        public override string Name { get; } = "N";

        public Knight(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override bool Move(Cell pozition, Figure[] figures, Cell move)
        {

            return true;
        }
    }
}