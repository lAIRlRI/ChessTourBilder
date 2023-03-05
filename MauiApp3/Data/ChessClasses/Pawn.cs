using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Пешка
    ///</summary>
    internal class Pawn : Figure
    {
        public override string Name { get; } = "";
        public bool IsMoving { get; set; } = false;

        public Pawn(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) { }

        public override bool Move(Cell pozition, Figure[] figures, Cell move)
        {
            return true;
        }
    }
}