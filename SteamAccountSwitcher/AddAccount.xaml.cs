using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SteamAccountSwitcher
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    
    public partial class AddAccount : Window
    {
        SteamAccount account;
        
        public SteamAccount Account
        {
            get { return account; }
        }

        public AddAccount()
        {
            account = new SteamAccount();
            InitializeComponent();
        }

        public AddAccount(SteamAccount editAccount)
        {
            InitializeComponent();
            account = editAccount;
            textBoxProfilename.Text = editAccount.Name;
            textBoxUsername.Text = editAccount.Username;
            textBoxPassword.Password = editAccount.Password;
            
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            account.Name = textBoxProfilename.Text;
            account.Username = textBoxUsername.Text;
            account.Password = textBoxPassword.Password;
            Close();
        }
    }
}
