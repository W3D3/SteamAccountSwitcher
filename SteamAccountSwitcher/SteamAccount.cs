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
        AccountType type;

        public SteamAccount()
        {
           
        }

        public SteamAccount(string username, string password)
        {
            this.name = username;
            this.username = username;
            this.password = password;
            this.type = AccountType.Main;
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

        public AccountType Type
        {
            get { return type; }
            set { this.type = value; }
        }

        public string Icon
        {
            get 
            {
                if (this.type == AccountType.Main)
                {
                    return "steam-ico-main.png";
                }
                if(this.type == AccountType.Smurf)
                {
                    return "steam-ico-smurf.png";
                }
                return null;
            }
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
