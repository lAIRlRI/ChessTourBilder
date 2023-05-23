using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Model
{
    internal class TableResult
    {
        public int EventID { get; set; }
        public int PlayerID { get; set; }
        public double Result { get; set; }
        public int ConsignmentID { get; set; }
    }
}
