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
    /// Interakční logika pro teamWindow.xaml
    /// </summary>
    public partial class teamWindow : Window
    {
        TeamModel tm;
        public teamWindow(TeamModel tm)
        {
            this.tm = tm;
            InitializeComponent();
            teamLabel.Content = tm.ToString();
            tm.Players = GlobalConfig.Connection.PlayerDao.GetPlayers_Team(tm.Team_Id);
            playersListView.ItemsSource = tm.Players;

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow().Show();
            this.Close();
        }

        private void joinTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            if (tm.Captain.Player_Id != GlobalConfig.CurrentPlayer.Player_Id)
            {
                MessageBox.Show("Nejsi kapitán");
                return;
            }

            new joinTournamentWindow(tm).Show();
            this.Close();
        }

        private void kickButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerModel pm = playersListView.SelectedItem as PlayerModel;
            if (pm == null)
            {
                MessageBox.Show("Hráč nenalezen");
                return;
            }

            if (tm.Captain.Player_Id != GlobalConfig.CurrentPlayer.Player_Id)
            {
                MessageBox.Show("Nejsi kapitán");
                return;
            }

            tm.Rating = GlobalConfig.Connection.TeamDao.KickPlayer(pm.Player_Id, tm.Team_Id);
            teamLabel.Content = tm.ToString();
            tm.Players.Remove(pm);
            playersListView.ItemsSource = tm.Players;
        }

        private void changeCaptainButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerModel pm = playersListView.SelectedItem as PlayerModel;
            if (pm == null)
            {
                MessageBox.Show("Hráč nenalezen");
                return;
            }

            if (tm.Captain.Player_Id != GlobalConfig.CurrentPlayer.Player_Id)
            {
                MessageBox.Show("Nejsi kapitán");
                return;
            }

            GlobalConfig.Connection.TeamDao.ChangeCaptain(pm.Player_Id, tm.Team_Id);
            tm.Captain = pm;
            teamLabel.Content = tm.ToString();
        }
    }
}
