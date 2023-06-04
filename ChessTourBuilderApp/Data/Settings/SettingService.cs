using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Settings
{
    internal class SettingService : ISettingServise
    {
        private const string IsFirstRunKey = "IsFirstRun";

        public bool IsFirstRun 
        {
            get => !Preferences.ContainsKey(IsFirstRunKey) || Preferences.Get(IsFirstRunKey, true);
            set => Preferences.Set(IsFirstRunKey, value);
        }
    }
}
