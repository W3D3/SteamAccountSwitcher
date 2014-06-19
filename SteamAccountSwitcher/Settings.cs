using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitcher
{
    class Settings
    {
        string installDir;

        public string InstallDir
        {
            get { return installDir; }
            set { installDir = value; }
        }

    }
}
