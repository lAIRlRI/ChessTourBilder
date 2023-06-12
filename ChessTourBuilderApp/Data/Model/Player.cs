namespace ChessTourBuilderApp.Data.Model
{
    internal class Player
    {
        public int? FIDEID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Passord { get; set; }
        public DateTime? Birthday { get; set; }
        public double? ELORating { get; set; }
        public string Contry { get; set; }
    }
}