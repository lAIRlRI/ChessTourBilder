using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.Model
{
    internal class Consignment
    {
        public int ConsignmentID { get; set; }
        public int TourID { get; set; }
        public int StatusID { get; set; }
        public DateTime? DateStart { get; set; }

        public ConsignmentPlayer whitePlayer;
        public ConsignmentPlayer blackPlayer;
    }
}   
