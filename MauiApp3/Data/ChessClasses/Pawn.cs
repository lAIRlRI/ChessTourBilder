using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    internal class Pawn : Figure
    {
        public override string Name { get; } = "P";

        public override void Move() => "";
    }
}
