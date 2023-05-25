using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using ChessTourBuilderApp.Data.Model;
using ChessTourBuilderApp.Data.Controler;
using System.Security.Cryptography;

namespace ChessTourBuilderApp.Data.HelpClasses
{
    internal class Helper
    {
        private static readonly Regex regex = new("[^а-яА-Яa-zA-Z]");

        public static Hashtable StringToInt = new(new Dictionary<char, int>()
            {
                {'A', 1 },{'B', 2 },{'C', 3 },{'D', 4 },
                {'E', 5 },{'F', 6 },{'G', 7 },{'H', 8 }
            }
        );

        public static string[] status = new string[3] { "Завершился", "Не начался", "Продолжается" };

        public static string[] IntToString = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

        private static string Text() => $"Поле не должно быть пустым";

        public static string FI() 
        {
            if (StaticResouses.IsPlayer) 
                return PlayerControler.nowPlayer.FirstName + " " + PlayerControler.nowPlayer.LastName;
            return OrganizerControler.nowOrganizer.FirstName + " " + OrganizerControler.nowOrganizer.MiddleName; 
        }

        public  static string GeneratePassword(int passwordLength)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var rng = RandomNumberGenerator.Create();
            var buffer = new byte[passwordLength * 4];
            rng.GetBytes(buffer);
            var result = new StringBuilder(passwordLength);

            for (int i = 0; i < passwordLength; i++)
            {
                var rnd = BitConverter.ToUInt32(buffer, i * 4);
                var idx = rnd % validChars.Length;

                result.Append(validChars[(int)idx]);
            }

            return result.ToString();
        }

        public static bool CheckDeleteButton() 
        {
            if (OrganizerControler.nowOrganizer == null) return false;
            return OrganizerControler.nowOrganizer.OrganizerID == EventControler.nowEvent.OrganizerID || OrganizerControler.nowOrganizer.Administrator != -1;
        } 

        public static async Task<string[]> CheckOrganizer(Organizer organizer)
        {
            string[] bools = new string[5];

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
            else if (!await OrganizerControler.GetLogin(organizer.Login))
                bools[3] = "Пользователь уже существует";

            if (string.IsNullOrWhiteSpace(organizer.Password))
                bools[4] = Text();

            return bools;
        }

        /// <summary>
        /// Проверка массива: все элементы null
        /// </summary>
        /// <param name="bools">массив сообщений</param>
        /// <returns>true - если все элементы null, иначе false</returns>
        public static bool CheckStringArray(string[] bools) => bools.All(p => p == null);

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

        public static async Task<string[]> CheckPlayer(Player player)
        {
            string[] bools = new string[7];

            if (player.FIDEID.ToString().Length != 7)
                bools[0] = "FIDEID должен состоять из 7 цифр";

            if (!await PlayerControler.GetLogin(player.FIDEID.ToString()))
                bools[0] = "Игрок уже существует";

            if (string.IsNullOrWhiteSpace(player.FirstName))
                bools[1] = Text();
            else if (regex.IsMatch(player.FirstName))
                bools[1] = "Поле может содержать только буквы";

            if (string.IsNullOrWhiteSpace(player.MiddleName))
                bools[2] = Text();
            else if (regex.IsMatch(player.MiddleName))
                bools[2] = "Поле может содержать только буквы";

            if (!string.IsNullOrWhiteSpace(player.LastName))
                if (regex.IsMatch(player.LastName))
                    bools[3] = "Поле может содержать только буквы";

            if (player.Birthday == null)
                bools[4] = Text();
            else if (player.Birthday > DateTime.Now)
                bools[4] = "не может больше меньше сегоднящней";

            if (player.ELORating == null)
                bools[5] = Text();
            else if (player.ELORating < 0)
                bools[5] = "ЕLO не может быть меньше 0";

            if (string.IsNullOrWhiteSpace(player.Contry))
                bools[6] = Text();
           
            return bools;
        }

        public static async Task<string[]> CheckPlayerUpAsync(Player player, int FIDEID)
        {
            string[] bools = new string[8];

            if (FIDEID != player.FIDEID) 
            {
                if (player.FIDEID.ToString().Length != 7)
                    bools[0] = "FIDEID должен состоять из 7 цифр";

                if (!await PlayerControler.GetLogin(player.FIDEID.ToString()))
                    bools[0] = "Игрок уже существует";
            }
            
            if (string.IsNullOrWhiteSpace(player.FirstName))
                bools[1] = Text();
            else if (regex.IsMatch(player.FirstName))
                bools[1] = "Поле может содержать только буквы";

            if (string.IsNullOrWhiteSpace(player.MiddleName))
                bools[2] = Text();
            else if (regex.IsMatch(player.MiddleName))
                bools[2] = "Поле может содержать только буквы";

            if (!string.IsNullOrWhiteSpace(player.LastName))
                if (regex.IsMatch(player.LastName))
                    bools[3] = "Поле может содержать только буквы";

            if (player.Birthday == null)
                bools[4] = Text();
            else if (player.Birthday > DateTime.Now)
                bools[4] = "не может больше меньше сегоднящней";

            if (player.ELORating == null)
                bools[5] = Text();
            else if (player.ELORating < 0)
                bools[5] = "ЕLO не может быть меньше 0";

            if (string.IsNullOrWhiteSpace(player.Contry))
                bools[6] = Text();

            if (player.FIDEID.ToString().Length != 7)
                bools[0] = "FIDEID должен состоять из 7 цифр";

            if (string.IsNullOrWhiteSpace(player.Passord))
                bools[7] = Text();

            return bools;
        }

        public static bool CheckConsignment(Consignment consignment, ref string[] bools)
        {
            if (consignment.DateStart == default(DateTime))
                bools[0] = Text();
            else if (consignment.DateStart < DateTime.Now)
                bools[0] = "Не может быть меньше сегоднящней";

            if (EventControler.nowEvent.DataStart < consignment.DateStart && EventControler.nowEvent.DataFinish > consignment.DateStart)
                bools[0] = "Партия должна проходить в рамках турнира";

            if (consignment.blackPlayer.PlayerID == 0)
                bools[1] = "Игрок не выбран";

            if (consignment.whitePlayer.PlayerID == 0)
                bools[2] = "Игрок не выбран";
            else if (consignment.blackPlayer.PlayerID == consignment.whitePlayer.PlayerID)
                bools[2] = "Человек не может играть сам с собой";

            return bools.All(p => p == null);
        }

        public static string[] CheckEvent(Event @event)
        {
            string[] bools = new string[5];
            if (string.IsNullOrWhiteSpace(@event.Name))
                bools[0] = Text();

            if (@event.PrizeFund == null) bools[1] = Text();
            else if (@event.PrizeFund <= 0)
                        bools[1] = "Призовой фонд должен быть больше 0";

            if (string.IsNullOrWhiteSpace(@event.LocationEvent))
                bools[4] = Text();

            if (@event.DataStart == null)
                bools[2] = Text();
            else if (@event.DataStart < DateTime.Now)
                bools[2] = "не может быть меньше сегоднящней";

            if (@event.DataFinish == null)
                bools[3] = Text();
            else if (@event.DataFinish < @event.DataStart)
                bools[3] = "не может быть меньше чем Дата начала";

            return bools;
        }
    }
}