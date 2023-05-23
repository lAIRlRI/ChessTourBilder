using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Model
{
    internal class ResultSheme
    {
        public static readonly Func<IDataReader, ResultSheme> mapper = r => new ResultSheme()
        {
            Points = r["Points"].ToString(),
            Fi = r["Fi"].ToString(),
        };
        public int Pozition { get; set; }
        public string Points { get; set; }
        public string Fi { get; set; }
    }
}
