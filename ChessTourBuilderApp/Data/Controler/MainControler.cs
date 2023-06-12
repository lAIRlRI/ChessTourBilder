using ChessTourBuilderApp.Data.Controler.ControlerServer;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class MainControler
    {
        public IConsignmentControler ConsignmentControler { get; private set; }
        public IEventControler EventControler { get; private set; }
        public IEventPlayerControler EventPlayerControler { get; private set; }
        public IPlayerControler PlayerControler { get; private set; }
        public IFigureTableControler FigureTableControler { get; private set; }
        public IMoveTableControler MoveTableControler { get; private set; }
        public IOrganizerControler OrganizerControler { get; private set; }
        public IResultTableControler ResultTableControler { get; private set; }
        public ITourControler TourControler { get; private set; }

        public bool IsServer;

        public MainControler(bool IsServer)
        {
            if (IsServer)
            {
                ConsignmentControler = new ConsignmentControler();
                EventControler = new EventControler();
                EventPlayerControler = new EventPlayerControler();
                PlayerControler = new PlayerControler();
                FigureTableControler = new FigureTableControler();
                MoveTableControler = new MoveTableControler();
                OrganizerControler = new OrganizerControler();
                ResultTableControler = new ResultTableControler();
                TourControler = new TourControler();
            }
            else 
            {
                ConsignmentControler = new ConsignmentControlerLite();
                EventControler = new EventControlerLite();
                EventPlayerControler = new EventPlayerControlerLite();
                PlayerControler = new PlayerControlerLite();
                FigureTableControler = new FigureTableControlerLite();
                MoveTableControler = new MoveTableControlerLite();
                OrganizerControler = new OrganizerControlerLite();
                ResultTableControler = new ResultTableControlerLite();
                TourControler = new TourControlerLite();
            }
            this.IsServer = IsServer;
        }
    }
}