using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Model
{
    internal class ConsignmentPlayer
    {
        public int ConsignmentPlayerID { get; set; }
        public int ConsignmentID { get; set; }
        public int PlayerID { get; set; }
        public bool IsWhile { get; set; }
        public double? Result { get; set; }

        public Player player;
    }
}