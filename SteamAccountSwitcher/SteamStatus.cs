using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitcher
{
    class SteamStatus
    {
        private static string API_URL = "http://is.steam.rip/api/v1/?request=IsSteamRip";
        private bool isSteamRip;
        private bool sessionsLogon;

        private void checkAPI()
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(API_URL);

                JObject steamStatus = JObject.Parse(json);
                isSteamRip = bool.Parse(steamStatus["result"]["isSteamRip"].ToString());
                sessionsLogon = bool.Parse(steamStatus["result"]["SessionsLogon"].ToString());
            }
        }
        public bool isSteamDown()
        {
            //refresh Data first
            checkAPI();
            //return if Steam is down
            return isSteamRip;
        }
    }
}
