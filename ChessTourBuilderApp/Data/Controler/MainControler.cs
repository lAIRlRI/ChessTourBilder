﻿using ChessTourBuilderApp.Data.Controler.ControlerServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MainControler(bool IsServer)
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
    }
}
