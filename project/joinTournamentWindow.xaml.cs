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
    /// Interakční logika pro joinTournamentWindow.xaml
    /// </summary>
    public partial class joinTournamentWindow : Window
    {
        TeamModel tm;
        public joinTournamentWindow(TeamModel tm)
        {
            this.tm = tm;
            InitializeComponent();
            tournamentsListView.ItemsSource = GlobalConfig.Connection.TournamentDao.GetTournaments_Except(tm.Team_Id, tm.Rating);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new teamWindow(tm).Show();
            this.Close();
        }

        private void joinButton_Click(object sender, RoutedEventArgs e)
        {
            TournamentModel tournament = tournamentsListView.SelectedItem as TournamentModel;
            if (tournament == null)
            {
                MessageBox.Show("Turnaj nenalezen");
            }
            GlobalConfig.Connection.TeamDao.JoinTournament(tm.Team_Id, tournament.Tournament_Id);

            new teamWindow(tm).Show();
            this.Close();
        }
    }
}
