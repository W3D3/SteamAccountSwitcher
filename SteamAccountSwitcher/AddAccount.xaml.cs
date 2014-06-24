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
            comboBoxType.ItemsSource = Enum.GetValues(typeof(AccountType));
        }

        public AddAccount(SteamAccount editAccount)
        {
            InitializeComponent();
            account = editAccount;
            comboBoxType.ItemsSource = Enum.GetValues(typeof(AccountType));

            comboBoxType.SelectedItem = editAccount.Type;
            textBoxProfilename.Text = editAccount.Name;
            textBoxUsername.Text = editAccount.Username;
            textBoxPassword.Password = editAccount.Password;
            
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                account.Type = (AccountType)comboBoxType.SelectedItem;
                account.Name = textBoxProfilename.Text;
                account.Username = textBoxUsername.Text;
                account.Password = textBoxPassword.Password;
            }
            catch
            {
                account = null;
            }
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (account != null)
            {
                if (account.Username == "" || account.Username == null)
                {
                    account = null;
                }
            }
        }
    }
}
