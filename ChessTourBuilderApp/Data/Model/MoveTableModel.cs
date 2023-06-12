namespace ChessTourBuilderApp.Data.Model
{
    internal class MoveTableModel
    {
        public string Move { get; set; }
        public string Pozition { get; set; }
        public int ID { get; set; }
        public int PlayerID { get; set; }
        public int ConsignmentID { get; set; }
        public int TourID { get; set; }
        public bool LastMove { get; set; } = false;
        public bool Winner { get; set; } = false;
    }
}
