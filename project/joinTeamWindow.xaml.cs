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
    /// Interakční logika pro joinTeamWindow.xaml
    /// </summary>
    public partial class joinTeamWindow : Window
    {
        public joinTeamWindow()
        {
            InitializeComponent();
            List<TeamModel> teams = GlobalConfig.Connection.TeamDao.GetTeams_Except(GlobalConfig.CurrentPlayer.Player_Id);
            teamListView.ItemsSource = teams;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow().Show();
            this.Close();
        }

        private void joinButton_Click(object sender, RoutedEventArgs e)
        {
            var team = teamListView.SelectedItem as TeamModel;
            if (team != null)
            {
                team.Rating = GlobalConfig.Connection.PlayerDao.JoinTeam(team.Team_Id);
                GlobalConfig.CurrentPlayer.Teams.Add(team);
                
            }

            new MenuWindow().Show();
            this.Close();
        }
    }
}
