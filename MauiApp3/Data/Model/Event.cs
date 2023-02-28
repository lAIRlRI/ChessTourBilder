using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data.Model
{
    internal class Event
    {
        public int EventID { get; set; }
        public string Name { get; set; }
        public int? PrizeFund { get; set; }
        public string LocationEvent { get; set; }
        public DateTime? DataStart { get; set; }
        public DateTime? DataFinish { get; set; }
        public int StatusID { get; set; }
        public int OrganizerID { get; set; }
        public bool IsPublic { get; set; }


        public string GetTableName()
        {
            return "[Result" + Name + EventID + "]";
        }
    }
}
