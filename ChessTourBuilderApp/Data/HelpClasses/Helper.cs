using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using ChessTourBuilderApp.Data.Model;
using System.Security.Cryptography;
using ChessTourBuilderApp.Pages;
using Player = ChessTourBuilderApp.Data.Model.Player;

namespace ChessTourBuilderApp.Data.HelpClasses
{
    internal class Helper
    {
        private const int _lenght = 30;

        private static readonly Regex regex = new("[^а-яА-Яa-zA-Z]");

        private static readonly Regex regexNoSpace = new("[^а-яА-Яa-zA-Z0-9]");

        public static Hashtable StringToInt = new(new Dictionary<char, int>()
            {
                {'A', 1 },{'B', 2 },{'C', 3 },{'D', 4 },
                {'E', 5 },{'F', 6 },{'G', 7 },{'H', 8 }
            }
        );

        public static string[] status = new string[3] { "Завершился", "Не начался", "Продолжается" };

        public static string[] IntToString = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

        private static string Text() => $"Поле не должно быть пустым";

        /// <summary>
        /// проверяет строку на пустоту, лишнии пробелы и допустимую длинную строки
        /// </summary>
        /// <param name="str">проверяемая строка</param>
        /// <param name="lenght">допустимая длинна</param>
        /// <returns>null - если строка прошла все проверки, иначе текст ошибки</returns>
        public static string CheckLenghtNumber(string str, int lenght = _lenght)
        {
            if (string.IsNullOrWhiteSpace(str))
                return Text();

            if (!Regex.IsMatch(str, @"^(?!.\s$)(?!.\s{2})(?!^\s).*$"))
                return "Уберите лишние пробелы.";
            
            if (str.Length > lenght)
                return $"Данные не должны превышать {lenght} символов.";

            return null;
        }

        public static string FI()
        {
            if (StaticResouses.IsPlayer)
                return StaticResouses.mainControler.PlayerControler.nowPlayer.FirstName + " " + StaticResouses.mainControler.PlayerControler.nowPlayer.LastName;
            return StaticResouses.mainControler.OrganizerControler.nowOrganizer.FirstName + " " + StaticResouses.mainControler.OrganizerControler.nowOrganizer.MiddleName;
        }

        public static string GeneratePassword(int passwordLength)
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
            if (StaticResouses.mainControler.OrganizerControler.nowOrganizer == null) return false;
            return StaticResouses.mainControler.OrganizerControler.nowOrganizer.OrganizerID == StaticResouses.mainControler.EventControler.nowEvent.OrganizerID || StaticResouses.mainControler.OrganizerControler.nowOrganizer.Administrator != -1;
        }

        /// <summary>
        /// проверяет строку на пустоту, лишнии пробелы и допустимую длинную строки+ проверка на то чтобы в строке были только буквы 
        /// </summary>
        /// <param name="str">проверяемая строка</param>
        /// <param name="lenght">допустимая длинна</param>
        /// <returns>null - если строка прошла все проверки, иначе текст ошибки</returns>
        private static string CheckTextParametr(string str, int lenght = _lenght)
        {
            string temp = CheckLenghtNumber(str, lenght);
            if (temp == null)
                if (regex.IsMatch(str))
                    temp = "Поле может содержать только буквы";
            return temp;
        }

        public static async Task<string[]> CheckOrganizer(Organizer organizer)
        {
            string[] bools = new string[5];

            bools[0] = CheckTextParametr(organizer.FirstName);

            bools[1] = CheckTextParametr(organizer.MiddleName);

            if (!string.IsNullOrWhiteSpace(organizer.LastName))
            {
                bools[2] = CheckTextParametr(organizer.LastName);
            }

            bools[3] = CheckLenghtNumber(organizer.Login);

            if (bools[3] == null)
            {
                if (!await StaticResouses.mainControler.OrganizerControler.GetLogin(organizer.Login))
                    bools[3] = "Пользователь уже существует";
            }
                        
            bools[4] = CheckLenghtNumber(organizer.Password);

            return bools;
        }

        public static async Task<string[]> CheckOrganizerUpdate(Organizer organizer, string login)
        {
            string[] bools = new string[5];

            bools[0] = CheckTextParametr(organizer.FirstName);

            bools[1] = CheckTextParametr(organizer.MiddleName);

            if (!string.IsNullOrWhiteSpace(organizer.LastName))
            {
                bools[2] = CheckTextParametr(organizer.LastName);
            }

            if (login != organizer.Login)
            {
                bools[3] = CheckLenghtNumber(organizer.Login);
                if (bools[3] == null)
                {
                    if (!await StaticResouses.mainControler.OrganizerControler.GetLogin(organizer.Login))
                        bools[3] = "Пользователь уже существует";
                }
            }

            bools[4] = CheckLenghtNumber(organizer.Password);

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
            else if (!await StaticResouses.mainControler.PlayerControler.GetLogin(player.FIDEID.ToString()))
                bools[0] = "Игрок уже существует";

            bools[1] = CheckTextParametr(player.FirstName);

            bools[2] = CheckTextParametr(player.MiddleName);

            if (!string.IsNullOrWhiteSpace(player.LastName))
                bools[3] = CheckTextParametr(player.LastName);

            if (player.Birthday == null)
                bools[4] = Text();
            else if (player.Birthday > DateTime.Now)
                bools[4] = "не может больше сегоднящней";

            if (player.ELORating == null)
                bools[5] = Text();
            else if (player.ELORating > 50000)
                bools[5] = "Не может быть больше 50000";
            else if (player.ELORating <= 0)
                bools[5] = "Не может быть меньше 0";

            bools[6] = CheckLenghtNumber(player.Contry);

            return bools;
        }

        public static async Task<string[]> CheckPlayerUpAsync(Player player, int FIDEID)
        {
            string[] bools = new string[8];

            if (FIDEID != player.FIDEID)
            {
                if (player.FIDEID.ToString().Length != 7)
                    bools[0] = "Должен состоять из 7 цифр";
                else if (!await StaticResouses.mainControler.PlayerControler.GetLogin(player.FIDEID.ToString()))
                    bools[0] = "Игрок уже существует";
            }

            bools[1] = CheckTextParametr(player.FirstName);

            bools[2] = CheckTextParametr(player.MiddleName);

            if (!string.IsNullOrWhiteSpace(player.LastName))
                bools[3] = CheckTextParametr(player.LastName);

            if (player.Birthday == null)
                bools[4] = Text();
            else if (player.Birthday > DateTime.Now)
                bools[4] = "не может больше сегоднящней";

            if (player.ELORating == null)
                bools[5] = Text();
            else if (player.ELORating > 50000)
                bools[5] = "Не может быть больше 50000";
            else if (player.ELORating <= 0)
                bools[5] = "Не может быть меньше 0";

            bools[6] = CheckLenghtNumber(player.Contry);

            bools[7] = CheckLenghtNumber(player.Passord);

            return bools;
        }

        public static bool CheckConsignment(Consignment consignment, ref string[] bools)
        {
            if (consignment.DateStart == default(DateTime))
                bools[0] = Text();
            else if (consignment.DateStart < DateTime.Now)
                bools[0] = "Дата меньше сегодняшней";

            if (!(StaticResouses.mainControler.EventControler.nowEvent.DataStart < consignment.DateStart && StaticResouses.mainControler.EventControler.nowEvent.DataFinish > consignment.DateStart))
                bools[0] = "Должна проходить в рамках турнира";

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

            bools[0] = CheckLenghtNumber(@event.Name);

            if (@event.PrizeFund == null) bools[1] = Text();
            else if (@event.PrizeFund <= 0)
                bools[1] = "Должен быть больше 0";

            bools[4] = CheckLenghtNumber(@event.LocationEvent, 50);

            if (@event.DataStart == null)
                bools[2] = Text();
            else if (@event.DataStart < DateTime.Now)
                bools[2] = "Дата меньше сегодняшней";

            if (@event.DataFinish == null)
                bools[3] = Text();
            else if (@event.DataFinish < @event.DataStart)
                bools[3] = "Дата меньше Даты начала";

            return bools;
        }
    }
}