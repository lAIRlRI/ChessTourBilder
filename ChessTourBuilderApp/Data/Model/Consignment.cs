namespace ChessTourBuilderApp.Data.Model
{
    internal class Consignment
    {
        public int ConsignmentID { get; set; }
        public int TourID { get; set; }
        public int StatusID { get; set; }
        public DateTime? DateStart { get; set; }
        public string GameMove { get; set; }
        public string TableName { get; set; }

        public ConsignmentPlayer whitePlayer;
        public ConsignmentPlayer blackPlayer;
    }
}
