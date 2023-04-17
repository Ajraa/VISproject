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
    /// Interakční logika pro MatchWindow.xaml
    /// </summary>
    public partial class MatchWindow : Window
    {
        TournamentModel tm;
        MatchModel mm;
        public MatchWindow(TournamentModel tm, MatchModel mm)
        {
            this.tm = tm;
            this.mm = mm;
            this.mm.Teams = GlobalConfig.Connection.TeamDao.GetTeams_Match(mm.Match_Id);
            InitializeComponent();
            statusComboBox.Items.Add("waiting");
            statusComboBox.Items.Add("ongoing");
            statusComboBox.Items.Add("finished");
            teamsListView.ItemsSource = GlobalConfig.Connection.TeamDao.GetTeams_MatchExcept(tm.Tournament_Id, mm.Match_Id);
            selectWinnerView.ItemsSource = GlobalConfig.Connection.TeamDao.GetTeams_Match(mm.Match_Id);
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            List<TeamModel> teams = teamsListView.SelectedItems.Cast<TeamModel>().ToList();
            string comboValue = statusComboBox.SelectedItem as string;
            if (comboValue != null) 
            {
                mm.Match_State = comboValue;
                GlobalConfig.Connection.MatchDao.UpdateMatch(mm);
            }

            foreach (TeamModel team in teams) 
            {
                GlobalConfig.Connection.MatchDao.addTeam(team.Team_Id, mm.Match_Id);
                mm.Teams.Add(team);
            }

            new tournamentWindow(tm).Show();
            this.Close();
        }

        private void setWinnerButton_Click(object sender, RoutedEventArgs e)
        {
            TeamModel team = selectWinnerView.SelectedItem as TeamModel;
            if (team == null)
            {
                MessageBox.Show("Tým neexistuje");
                return;
            }
            GlobalConfig.Connection.MatchDao.setWinner(team.Team_Id, mm.Match_Id);

            new tournamentWindow(tm).Show();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new tournamentWindow(tm).Show();
            this.Close();
        }
    }
}
