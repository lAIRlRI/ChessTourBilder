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
        public Pawn(string poziton, bool IsWhile) : base(poziton, IsWhile) { }

        public override void Move()
        {
            return;
        }
    }
}