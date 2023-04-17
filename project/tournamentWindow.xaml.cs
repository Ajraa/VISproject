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
    /// Interakční logika pro tournamentWindow.xaml
    /// </summary>
    public partial class tournamentWindow : Window
    {
        TournamentModel tm;
        bool teams;
        public tournamentWindow(TournamentModel tm)
        {
            this.tm = tm;
            this.tm.Teams = GlobalConfig.Connection.TeamDao.GetTeams_Tournament(tm.Tournament_Id);
            this.tm.Matches = GlobalConfig.Connection.MatchDao.GetMatches_Tournament(tm.Tournament_Id);
            InitializeComponent();
            tournamentLabel.Content = tm.ToString();
            tournamentListView.ItemsSource = tm.Teams;
            this.teams = true;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            new founderMenuWindow().Show();
            this.Close();
        }

        private void swapButton_Click(object sender, RoutedEventArgs e)
        {
            if (teams)
            {
                tournamentListView.ItemsSource = tm.Matches;
                this.teams = false;
            }
            else {
                tournamentListView.ItemsSource = tm.Teams;
                this.teams = true;
            }

        }

        private void addMatchButton_Click(object sender, RoutedEventArgs e)
        {
            MatchModel mm = new MatchModel();
            mm.Tournament = tm;
            mm.Match_State = "waiting";
            mm.Match_Id = GlobalConfig.Connection.MatchDao.CreateMatch(mm);
            tm.Matches.Add(mm);
            tournamentListView.ItemsSource = tm.Matches;
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (teams)
            {
                TeamModel team = tournamentListView.SelectedItem as TeamModel;
                if (team == null)
                {
                    MessageBox.Show("Tým neexistuje");
                    return;
                }
                GlobalConfig.Connection.TournamentDao.removeTeam(tm.Tournament_Id, team.Team_Id);
                tm.Teams.Remove(team);
            }
            else { 
                MatchModel match = tournamentListView.SelectedItem as MatchModel;
                if (match == null)
                {
                    MessageBox.Show("Zápas neexistuje");
                    return;
                }
                GlobalConfig.Connection.TournamentDao.removeMatch(match.Match_Id);
                tm.Matches.Remove(match);
            }
        }

        private void tournamentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (teams)
            {
                return;
            }
            else {
                MatchModel match = tournamentListView.SelectedItem as MatchModel;
                if (match == null)
                {
                    MessageBox.Show("Zápas neexistuje");
                    return;
                }
                new MatchWindow(tm, match).Show();
                this.Close();
           }
        }
    }
}
