CREATE TABLE Status(
    StatusID INT NOT NULL IDENTITY(1,1),
    Name NVARCHAR(20) NOT NULL
);
ALTER TABLE
    Status ADD CONSTRAINT status_status_primary PRIMARY KEY(StatusID);
CREATE TABLE Player(
    FIDEID INT NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NULL,
    Birthday DATE NOT NULL,
    ELORating FLOAT NOT NULL,
    Contry NVARCHAR(50) NOT NULL,
	Image varbinary(max) NULL
);
ALTER TABLE
    Player ADD CONSTRAINT player_fideid_primary PRIMARY KEY(FIDEID);
CREATE TABLE EventPlayer(
    EventPlayerID INT NOT NULL IDENTITY(1,1),
    EventID INT NOT NULL,
    PlayerID INT NOT NULL,
	TopPlece INT NULL
);
ALTER TABLE
    EventPlayer ADD CONSTRAINT eventplayer_eventplayerid_primary PRIMARY KEY(EventPlayerID);
CREATE TABLE Event(
    EventID INT NOT NULL IDENTITY(1,1),
    Name NVARCHAR(150) NOT NULL,
    PrizeFund INT NOT NULL,
	LocationEvent NVARCHAR(200) NOT NULL,
    DataStart DATETIME NOT NULL,
    DataFinish DATETIME NOT NULL,
    StatusID INT NOT NULL,
    OrganizerID INT NOT NULL,
	IsPublic BIT NOT NULL,
	TypeEvent BIT NOT NULL,
	Image varbinary(max) NULL
);
ALTER TABLE
    Event ADD CONSTRAINT event_eventid_primary PRIMARY KEY(EventID);
CREATE TABLE Tour(
    TourID INT NOT NULL IDENTITY(1,1),
    NameTour NVARCHAR(50) NOT NULL,
    EventID INT NOT NULL
);
ALTER TABLE
    Tour ADD CONSTRAINT tour_tourid_primary PRIMARY KEY(TourID);
CREATE TABLE Consignment(
    ConsignmentID INT NOT NULL IDENTITY(1,1),
    DateStart DATETIME NOT NULL,
    TourID INT NOT NULL,
    StatusID INT NOT NULL,
	GameMove NVARCHAR(max) NULL,
	TableName NVARCHAR(max) NULL
);
ALTER TABLE
    Consignment ADD CONSTRAINT consignment_consignmentid_primary PRIMARY KEY(ConsignmentID);
CREATE TABLE ConsignmentPlayer(
    ConsignmentPlayerID INT NOT NULL IDENTITY(1,1),
    ConsignmentID INT NOT NULL,
    PlayerID INT NOT NULL,
    IsWhile BIT NOT NULL,
    Result FLOAT NULL
);
ALTER TABLE
    ConsignmentPlayer ADD CONSTRAINT consignmentplayer_consignmentplayerid_primary PRIMARY KEY(ConsignmentPlayerID);	
CREATE TABLE Organizer(
    OrganizerID INT NOT NULL IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NULL,
    Login NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
	Image varbinary(max) NULL
);
ALTER TABLE
    Organizer ADD CONSTRAINT organizer_organizerid_primary PRIMARY KEY(OrganizerID);
CREATE TABLE Administrator(
    AdministratorID INT NOT NULL IDENTITY(1,1),
    OrganizerID INT NOT NULL
);
ALTER TABLE
    Administrator ADD CONSTRAINT administrator_administratorid_primary PRIMARY KEY(AdministratorID);
ALTER TABLE
    Event ADD CONSTRAINT event_statusid_foreign FOREIGN KEY(StatusID) REFERENCES Status(StatusID);
ALTER TABLE
    EventPlayer ADD CONSTRAINT eventplayer_playerid_foreign FOREIGN KEY(PlayerID) REFERENCES Player(FIDEID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE
    ConsignmentPlayer ADD CONSTRAINT consignmentplayer_playerid_foreign FOREIGN KEY(PlayerID) REFERENCES Player(FIDEID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE
    EventPlayer ADD CONSTRAINT eventplayer_eventid_foreign FOREIGN KEY(EventID) REFERENCES Event(EventID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE
    ConsignmentPlayer ADD CONSTRAINT consignmentplayer_consignmentid_foreign FOREIGN KEY(ConsignmentID) REFERENCES Consignment(ConsignmentID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE
    Administrator ADD CONSTRAINT administrator_organizerid_foreign FOREIGN KEY(OrganizerID) REFERENCES Organizer(OrganizerID);
ALTER TABLE
    Event ADD CONSTRAINT event_organizerid_foreign FOREIGN KEY(OrganizerID) REFERENCES Organizer(OrganizerID);
ALTER TABLE
    Consignment ADD CONSTRAINT consignment_statusid_foreign FOREIGN KEY(StatusID) REFERENCES Status(StatusID);
ALTER TABLE
    Consignment ADD CONSTRAINT consignment_tourid_foreign FOREIGN KEY(TourID) REFERENCES Tour(TourID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE
    Tour ADD CONSTRAINT tour_eventid_foreign FOREIGN KEY(EventID) REFERENCES Event(EventID)ON DELETE CASCADE ON UPDATE CASCADE;

insert into Status values ('Завершился'),('Не начался'),('Продолжается')