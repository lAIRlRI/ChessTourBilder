using MauiApp3.Data.Controler;
using MauiApp3.Data.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MauiApp3.Data
{
    internal class Helper
    {
        static Regex regex = new Regex("[^а-яА-Яa-zA-Z]");

        public static Hashtable StringToInt = new Hashtable(new Dictionary<char, int>()
            {
                {'A', 1 },
                {'B', 2 },
                {'C', 3 },
                {'D', 4 },
                {'E', 5 },
                {'F', 6 },
                {'G', 7 },
                {'H', 8 }
            }
        );

        public static string[] IntToString = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

        private static string Text() => $"Поле не должно быть пустым";
        private static string Text(string str) => $"Поле {str} не должно быть пустым";
        public static string FI() => OrganizerControler.nowOrganizer.FirstName + " " + OrganizerControler.nowOrganizer.MiddleName;
        public static bool CheckDeleteButton() => OrganizerControler.nowOrganizer.OrganizerID == EventControler.nowEvent.OrganizerID || OrganizerControler.nowOrganizer.Administrator != -1;

        public static bool CheckOrganizer(Organizer organizer, ref string[] bools)
        {
            if (string.IsNullOrWhiteSpace(organizer.FirstName))
                bools[0] = Text();
            else if (regex.IsMatch(organizer.FirstName))
                bools[0] = "Поле может содержать только буквы";

            if (string.IsNullOrWhiteSpace(organizer.MiddleName))
                bools[1] = Text();
            else if (regex.IsMatch(organizer.MiddleName))
                bools[1] = "Поле может содержать только буквы";

            if (!string.IsNullOrWhiteSpace(organizer.LastName))
                if (regex.IsMatch(organizer.LastName))
                    bools[2] = "Поле может содержать только буквы";

            if (string.IsNullOrWhiteSpace(organizer.Login))
                bools[3] = Text();
            else if (OrganizerControler.Get().Where(p => p.Login == organizer.Login).FirstOrDefault() != default(Organizer))
                bools[3] = "Пользователь уже существует";

            if (string.IsNullOrWhiteSpace(organizer.Password))
                bools[4] = Text();


            return bools.All(p => p == null);
        }

        public static bool CheckDB(string[] values, ref string[] bools)
        {
            if (string.IsNullOrWhiteSpace(values[0]))
                bools[0] = Text();

            if (string.IsNullOrWhiteSpace(values[1]))
                bools[1] = Text();

            if (string.IsNullOrWhiteSpace(values[2]))
                bools[2] = Text();

            if (string.IsNullOrWhiteSpace(values[3]))
                bools[3] = Text();

            return bools.All(p => p == null);
        }

        public static bool CheckPlayer(Player player, ref string[] bools)
        {
            if (player.FIDEID.ToString().Length != 7)
                bools[0] = "FIDEID должен состоять из 7 цифр";

            if (PlayerControler.Get().Where(p => p.FIDEID == player.FIDEID).FirstOrDefault() != default(Player))
                bools[0] = "Игрок уже существует";

            if (string.IsNullOrWhiteSpace(player.FirstName))
                bools[1] = Text();

            if (string.IsNullOrWhiteSpace(player.MiddleName))
                bools[2] = Text();

            if (player.Birthday == default(DateTime))
                bools[4] = Text();

            if (player.ELORating < 0)
                bools[3] = "ЕLO не может быть меньше 0";

            if (string.IsNullOrWhiteSpace(player.Contry))
                bools[6] = Text();

            return bools.All(p => p == null);
        }

        public static bool CheckConsignment(Consignment consignment, ref string[] bools)
        {
            if (consignment.DateStart == default(DateTime))
                bools[0] = Text();
            else if (consignment.DateStart < DateTime.Now)
                bools[0] = "Не может быть меньше сегоднящней";

            if (consignment.blackPlayer.PlayerID == 0)
                bools[1] = "Игрок не выбран";

            if (consignment.whitePlayer.PlayerID == 0)
                bools[2] = "Игрок не выбран";
            else if (consignment.blackPlayer.PlayerID == consignment.whitePlayer.PlayerID)
                bools[2] = "Человек не может играть сам с собой";

            return bools.All(p => p == null);
        }

        public static bool CheckEvent(Event @event, ref string[] bools)
        {
            if (string.IsNullOrWhiteSpace(@event.Name))
                bools[0] = Text();

            if (@event.PrizeFund <= 0)
                bools[1] = "Призовой фонд должен быть больше 0";

            if (string.IsNullOrWhiteSpace(@event.LocationEvent))
                bools[2] = Text();

            if (@event.DataStart == default(DateTime))
                bools[3] = Text();
            else if (@event.DataStart < DateTime.Now)
                bools[3] = "не может быть меньше сегоднящней";

            if (@event.DataFinish == default(DateTime))
                bools[4] = Text();
            else if (@event.DataFinish < @event.DataStart)
                bools[4] = "не может быть меньше чем Дата начала";

            return bools.All(p => p == null);
        }

        public static string Tablels = "CREATE TABLE Status(" +
            "StatusID INT NOT NULL IDENTITY(1,1)," +
            "Name NVARCHAR(20) NOT NULL); " +
            "ALTER TABLE Status ADD CONSTRAINT status_status_primary PRIMARY KEY(StatusID); " +
            "CREATE TABLE Player(" +
            "FIDEID INT NOT NULL," +
            "FirstName NVARCHAR(50) NOT NULL," +
            "MiddleName NVARCHAR(50) NOT NULL," +
            "LastName NVARCHAR(50) NULL," +
            "Birthday DATE NOT NULL," +
            "ELORating FLOAT NOT NULL," +
            "Contry NVARCHAR(50) NOT NULL," +
            "Image varbinary(max) NULL); " +
            "ALTER TABLE Player ADD CONSTRAINT player_fideid_primary PRIMARY KEY(FIDEID); " +
            "CREATE TABLE EventPlayer(EventPlayerID INT NOT NULL IDENTITY(1,1),EventID INT NOT NULL,PlayerID INT NOT NULL,TopPlece INT NULL); " +
            "ALTER TABLE EventPlayer ADD CONSTRAINT eventplayer_eventplayerid_primary PRIMARY KEY(EventPlayerID); " +
            "CREATE TABLE Event(" +
            "EventID INT NOT NULL IDENTITY(1,1)," +
            "Name NVARCHAR(150) NOT NULL," +
            "PrizeFund INT NOT NULL," +
            "LocationEvent NVARCHAR(200) NOT NULL," +
            "DataStart DATE NOT NULL," +
            "DataFinish DATE NOT NULL," +
            "StatusID INT NOT NULL," +
            "OrganizerID INT NOT NULL," +
            "IsPublic BIT NOT NULL," +
            "TypeEvent BIT NOT NULL," +
            "Image varbinary(max) NULL);" +
            "ALTER TABLE Event ADD CONSTRAINT event_eventid_primary PRIMARY KEY(EventID);" +
            "CREATE TABLE Tour(TourID INT NOT NULL IDENTITY(1,1),NameTour NVARCHAR(50) NOT NULL,EventID INT NOT NULL);" +
            "ALTER TABLE Tour ADD CONSTRAINT tour_tourid_primary PRIMARY KEY(TourID);" +
            "CREATE TABLE Consignment(" +
            "ConsignmentID INT NOT NULL IDENTITY(1,1)," +
            "DateStart DATETIME NOT NULL," +
            "TourID INT NOT NULL," +
            "StatusID INT NOT NULL," +
            "GameMove NVARCHAR(max) NULL," +
            "TableName NVARCHAR(max) NULL);" +
            "ALTER TABLE Consignment ADD CONSTRAINT consignment_consignmentid_primary PRIMARY KEY(ConsignmentID);" +
            "CREATE TABLE ConsignmentPlayer(" +
            "ConsignmentPlayerID INT NOT NULL IDENTITY(1,1)," +
            "ConsignmentID INT NOT NULL,PlayerID INT NOT NULL,IsWhile BIT NOT NULL,Result FLOAT NULL); " +
            "ALTER TABLE ConsignmentPlayer ADD CONSTRAINT consignmentplayer_consignmentplayerid_primary PRIMARY KEY(ConsignmentPlayerID); " +
            "CREATE TABLE Organizer(" +
            "OrganizerID INT NOT NULL IDENTITY(1,1)," +
            "FirstName NVARCHAR(50) NOT NULL," +
            "MiddleName NVARCHAR(50) NOT NULL," +
            "LastName NVARCHAR(50) NULL," +
            "Login NVARCHAR(100) NOT NULL," +
            "Password NVARCHAR(100) NOT NULL," +
            "Image varbinary(max) NULL); " +
            "ALTER TABLE Organizer ADD CONSTRAINT organizer_organizerid_primary PRIMARY KEY(OrganizerID); " +
            "CREATE TABLE Administrator(AdministratorID INT NOT NULL IDENTITY(1,1),OrganizerID INT NOT NULL); " +
            "ALTER TABLE Administrator ADD CONSTRAINT administrator_administratorid_primary PRIMARY KEY(AdministratorID); " +
            "ALTER TABLE Event ADD CONSTRAINT event_statusid_foreign FOREIGN KEY(StatusID) REFERENCES Status(StatusID); " +
            "ALTER TABLE EventPlayer ADD CONSTRAINT eventplayer_playerid_foreign FOREIGN KEY(PlayerID) REFERENCES Player(FIDEID) ON DELETE CASCADE ON UPDATE CASCADE; " +
            "ALTER TABLE ConsignmentPlayer ADD CONSTRAINT consignmentplayer_playerid_foreign FOREIGN KEY(PlayerID) REFERENCES Player(FIDEID) ON DELETE CASCADE ON UPDATE CASCADE; " +
            "ALTER TABLE EventPlayer ADD CONSTRAINT eventplayer_eventid_foreign FOREIGN KEY(EventID) REFERENCES Event(EventID) ON DELETE CASCADE ON UPDATE CASCADE; " +
            "ALTER TABLE ConsignmentPlayer ADD CONSTRAINT consignmentplayer_consignmentid_foreign FOREIGN KEY(ConsignmentID) REFERENCES Consignment(ConsignmentID) ON DELETE CASCADE ON UPDATE CASCADE; " +
            "ALTER TABLE Administrator ADD CONSTRAINT administrator_organizerid_foreign FOREIGN KEY(OrganizerID) REFERENCES Organizer(OrganizerID); " +
            "ALTER TABLE Event ADD CONSTRAINT event_organizerid_foreign FOREIGN KEY(OrganizerID) REFERENCES Organizer(OrganizerID); " +
            "ALTER TABLE Consignment ADD CONSTRAINT consignment_statusid_foreign FOREIGN KEY(StatusID) REFERENCES Status(StatusID); " +
            "ALTER TABLE Consignment ADD CONSTRAINT consignment_tourid_foreign FOREIGN KEY(TourID) REFERENCES Tour(TourID) ON DELETE CASCADE ON UPDATE CASCADE; " +
            "ALTER TABLE Tour ADD CONSTRAINT tour_eventid_foreign FOREIGN KEY(EventID) REFERENCES Event(EventID)ON DELETE CASCADE ON UPDATE CASCADE; " +
            "insert into Status values ('Завершился'),('Не начался'),('Продолжается')";
    }
}