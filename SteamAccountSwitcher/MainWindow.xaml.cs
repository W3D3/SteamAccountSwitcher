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

namespace SteamAccountSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<SteamAccount> accounts;
        public string installPath;
        public MainWindow()
        {
            InitializeComponent();
            accounts = new List<SteamAccount>();

            ReadAccountsFromFile();

            listBoxAccounts.ItemsSource = accounts;
            listBoxAccounts.Items.Refresh();
        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            Steam.LogoutSteam();
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
            MessageBox.Show(xmlAccounts);
            System.IO.StreamWriter file = new System.IO.StreamWriter("H:\\test.ini");
            file.Write(xmlAccounts);
            file.Close();
        }

        public void ReadAccountsFromFile()
        {
            try
            {
                string text = System.IO.File.ReadAllText("H:\\test.ini");
                accounts = FromXML<List<SteamAccount>>(text);
            }
            catch
            {}
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
            Steam.StartSteamAccount(selectedAcc);
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
            MessageBoxResult dialogResult = MessageBox.Show("Sure", "Some Title", MessageBoxButton.YesNo);
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
