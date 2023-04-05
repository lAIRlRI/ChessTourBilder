using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Model
{
    internal class EventPlayer
    {
        public int EventPlayerID { get; set; }
        public int EventID { get; set; }
        public int PlayerID { get; set; }
        public int? TopPlece { get; set; }
    }
}
