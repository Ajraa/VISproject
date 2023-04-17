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
using DataLibrary;
using DataLibrary.Model;

namespace project
{
    /// <summary>
    /// Interakční logika pro founderMenuWindow.xaml
    /// </summary>
    public partial class founderMenuWindow : Window
    {
        public founderMenuWindow()
        {
            InitializeComponent();
            tournamentListView.ItemsSource = GlobalConfig.CurrentFounder.TournamentList;
            founderLabel.Content = GlobalConfig.CurrentFounder.ToString();
        }

        private void createTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            new createTournamentWindow().Show();
            this.Close();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.CurrentFounder = null;
            new ChooseLoginWindow().Show();
            this.Close();
        }

        private void tournamentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TournamentModel tm = tournamentListView.SelectedItem as TournamentModel;
            if (tm == null)
            {
                MessageBox.Show("Turnaj neexistuje");
                return;
            }

            tm.Matches = GlobalConfig.Connection.MatchDao.GetMatches_Tournament(tm.Tournament_Id);
            tm.Teams = GlobalConfig.Connection.TeamDao.GetTeams_Tournament(tm.Tournament_Id);
            new tournamentWindow(tm).Show();
            this.Close();
        }
    }
}
