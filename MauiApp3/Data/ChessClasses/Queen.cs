using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    ///<summary>
    ///Фигура Ферзь
    ///</summary>
    internal class Queen : Figure
    {
        private Rook rook;
        private Bishop bishop;

        public override string Name { get; } = "Q";


        public Queen(string poziton, bool IsWhile, int ID) : base(poziton, IsWhile, ID) 
        {
            rook = new Rook(poziton,IsWhile,ID);
            bishop = new Bishop(poziton,IsWhile,ID);
        }

        public override bool Move(Cell pozition, Figure[] figures, Cell move)
        {
            if(!rook.Move(pozition,figures, move)) return false;
            if(!bishop.Move(pozition,figures, move)) return false;
            return true;
        }
    }
}