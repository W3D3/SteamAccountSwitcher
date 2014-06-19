using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace SteamAccountSwitcher
{
    class Steam
    {
        public static bool StartSteamAccount(SteamAccount a)
        {
            LogoutSteam();
            Process p = new Process();
            if (File.Exists(@"C:\Steam\Steam.exe"))
            {
                p.StartInfo = new ProcessStartInfo(@"C:\Steam\Steam.exe", a.getStartParameters());
                p.Start();
                return true;
            }
            return false;
        }


        public static bool LogoutSteam()
        {
            Process p = new Process();
            if (File.Exists(@"C:\Steam\Steam.exe"))
            {
                p.StartInfo = new ProcessStartInfo(@"C:\Steam\Steam.exe", "-shutdown");
                p.Start();
                return true;
            }

            return false;
            
        }
    }
}
