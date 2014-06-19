using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitcher
{
    public class SteamAccount
    {
        string name;
        string username;
        string password;

        public SteamAccount()
        {
           
        }
        public SteamAccount(string username, string password)
        {
            this.name = username;
            this.username = username;
            this.password = password;
        }

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        public string Username
        {
            get { return username; }
            set { this.username = value; }
        }

        public string Password
        {
            get { return password; }
            set { this.password = value; }
        }

        public string getStartParameters()
        {
            return "-login " + this.username + " " + this.password;
        }
        public override string ToString()
        {
            return name + "~ (user: " + username + ")";
        }
    }
}
