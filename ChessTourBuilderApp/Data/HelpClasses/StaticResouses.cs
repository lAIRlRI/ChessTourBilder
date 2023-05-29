using ChessTourBuilderApp.Data.Controler;
using ChessTourBuilderApp.Data.DataBases;

namespace ChessTourBuilderApp.Data.HelpClasses
{
    internal class StaticResouses
    {
        public static IDBQ dBQ;

        public static MainControler mainControler = new(true);

        public static bool IsPlayer = false;
    }
}