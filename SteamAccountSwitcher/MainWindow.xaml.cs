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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using Microsoft.Win32;

namespace SteamAccountSwitcher
{
    /// ****
    /// SteamAccountSwitcher
    /// Copyright by Christoph Wedenig
    /// ****
    
    public partial class MainWindow : Window
    {
        AccountList accountList;
        Steam steam;

        string settingsSave;

        public MainWindow()
        {
            InitializeComponent();
            accountList = new AccountList();
            
            //Get directory of Executable
            settingsSave = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).TrimStart(@"file:\\".ToCharArray());

            try
            {
                ReadAccountsFromFile();
            }
            catch
            {
                //Maybe create file?
            }

            

            listBoxAccounts.ItemsSource = accountList.Accounts;
            listBoxAccounts.Items.Refresh();

            if (accountList.InstallDir == "" || (accountList.InstallDir == null))
            {
                accountList.InstallDir = SelectSteamFile(@"C:\Program Files (x86)\Steam");
                if(accountList.InstallDir == null)
                {
                    MessageBox.Show("You cannot use SteamAccountSwitcher without selecting your Steam.exe. Program will close now.", "Steam missing", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }

            steam = new Steam(accountList.InstallDir);
            
        }

        private string SelectSteamFile(string initialDirectory)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "Steam |steam.exe";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Select your Steam Installation";
            return (dialog.ShowDialog() == true)
               ? dialog.FileName : null;
        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            steam.LogoutSteam();
        }

        private void buttonAddAccount_Click(object sender, RoutedEventArgs e)
        {
            AddAccount newAccWindow = new AddAccount();
            newAccWindow.Owner = this;
            newAccWindow.ShowDialog();

            if (newAccWindow.Account != null)
            {
                accountList.Accounts.Add(newAccWindow.Account);

                listBoxAccounts.Items.Refresh();
            }
        }

        public void WriteAccountsToFile()
        {
            string xmlAccounts = this.ToXML<AccountList>(accountList);
            System.IO.StreamWriter file = new System.IO.StreamWriter(settingsSave + "\\accounts.ini");
            file.Write(Crypto.Encrypt(xmlAccounts));
            file.Close();
        }

        public void ReadAccountsFromFile()
        {
            string text = System.IO.File.ReadAllText(settingsSave + "\\accounts.ini");
            accountList = FromXML<AccountList>(Crypto.Decrypt(text));
        }

        public static T FromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public string ToXML<T>(T obj)
        {
            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }

        private void listBoxAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SteamAccount selectedAcc = (SteamAccount)listBoxAccounts.SelectedItem;
            steam.StartSteamAccount(selectedAcc);
        }


        private void buttonEditAccount_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxAccounts.SelectedItem != null)
            {
                AddAccount newAccWindow = new AddAccount((SteamAccount)listBoxAccounts.SelectedItem);
                newAccWindow.Owner = this;
                newAccWindow.ShowDialog();

                if (newAccWindow.Account.Username != "" && newAccWindow.Account.Password != "")
                {
                    accountList.Accounts[listBoxAccounts.SelectedIndex] = newAccWindow.Account;

                    listBoxAccounts.Items.Refresh();
                }
            }
        }

        private void buttonDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            Button itemClicked = (Button)e.Source;

            SteamAccount selectedAcc = (SteamAccount)itemClicked.DataContext;
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete the '" + selectedAcc.Name + "' account?", "Delete Account", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                accountList.Accounts.Remove((SteamAccount)listBoxAccounts.SelectedItem);
                listBoxAccounts.Items.Refresh();
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                //do something else
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            WriteAccountsToFile();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image itemClicked = (Image)e.Source;

            SteamAccount selectedAcc = (SteamAccount)itemClicked.DataContext;
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete the '" + selectedAcc.Name + "' account?", "Delete Account", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                accountList.Accounts.Remove((SteamAccount)listBoxAccounts.SelectedItem);
                listBoxAccounts.Items.Refresh();
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                //do something else
            }
        }

    }
}
