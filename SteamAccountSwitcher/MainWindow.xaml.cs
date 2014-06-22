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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<SteamAccount> accounts;
        public string installPath;
        Steam steam;
        string settingsSave;

        public MainWindow()
        {
            InitializeComponent();
            accounts = new List<SteamAccount>();
            steam = new Steam(@"C:\Program Files (x86)\Steam\");
            settingsSave = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).TrimStart(@"file:\\".ToCharArray());


            ReadAccountsFromFile();

            listBoxAccounts.ItemsSource = accounts;
            listBoxAccounts.Items.Refresh();

            steam.InstallDir = SelectSteamFile(steam.InstallDir);
        }

        private string SelectSteamFile(string initialDirectory)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "Steam Exe (steam.exe)|steam.exe";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Select your Steam.exe";
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
            newAccWindow.ShowDialog();

            accounts.Add(newAccWindow.Account);
            
            listBoxAccounts.Items.Refresh();
        }

        public void WriteAccountsToFile()
        {
            string xmlAccounts = this.ToXML<List<SteamAccount>>(accounts);
            System.IO.StreamWriter file = new System.IO.StreamWriter(settingsSave + "\\accounts.ini");
            file.Write(xmlAccounts);
            file.Close();
        }

        public void ReadAccountsFromFile()
        {
            try
            {
                string text = System.IO.File.ReadAllText(settingsSave + "\\accounts.ini");
                accounts = FromXML<List<SteamAccount>>(text);
            }
            catch(Exception x)
            {
                MessageBox.Show(x.ToString());
            }
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

            AddAccount newAccWindow = new AddAccount((SteamAccount)listBoxAccounts.SelectedItem);
            newAccWindow.ShowDialog();

            accounts[listBoxAccounts.SelectedIndex] = newAccWindow.Account;
            
            listBoxAccounts.Items.Refresh();
        }

        private void buttonDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            SteamAccount selectedAcc = (SteamAccount)listBoxAccounts.SelectedItem;
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete the'" + selectedAcc.Name + "' Account?", "Delete Account", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                accounts.Remove((SteamAccount)listBoxAccounts.SelectedItem);
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

    }
}
