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
        public static bool IsSteamRunning()
        {
            Process[] pname = Process.GetProcessesByName("steam");
            if (pname.Length == 0)
                return false;
            else
                return true;
        }

        public static void KillSteam()
        {
            Process [] proc = Process.GetProcessesByName("steam");
	        proc[0].Kill();
        }
        public static bool StartSteamAccount(SteamAccount a)
        {
            bool finished = false;

            if(IsSteamRunning())
            {
                KillSteam();
            }

            while (finished == false)
            {
                if (IsSteamRunning() == false)
                {
                    Process p = new Process();
                    if (File.Exists(@"C:\Steam\Steam.exe"))
                    {
                        p.StartInfo = new ProcessStartInfo(@"C:\Steam\Steam.exe", a.getStartParameters());
                        p.Start();
                        finished = true;
                        return true;
                    }
                }
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
