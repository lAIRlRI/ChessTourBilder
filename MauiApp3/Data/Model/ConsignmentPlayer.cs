using MauiApp3.Data.Controler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.Model
{
    internal class ConsignmentPlayer
    {
        public int ConsignmentPlayerID { get; set; }
        public int ConsignmentID { get; set; }
        public int PlayerID { get; set; }
        public bool IsWhile { get; set; }
        public double? Result { get; set; }
        public double? Score { get; set; }

        public Player player;
    }
}
