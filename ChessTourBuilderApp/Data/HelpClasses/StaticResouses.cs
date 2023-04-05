using ChessTourBuilderApp.Data.DataBases.Interfeses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.HelpClasses
{
    internal class StaticResouses
    {
        public static IDataBase dataBase = new DataBases.DataBase();
    }
}
