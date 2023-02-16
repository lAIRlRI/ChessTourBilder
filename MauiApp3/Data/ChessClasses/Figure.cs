using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.ChessClasses
{
    internal abstract class Figure
    {
        public abstract string Name { get; }

        public string Pozition { get; set; }

        public abstract void Move();
    }
}
