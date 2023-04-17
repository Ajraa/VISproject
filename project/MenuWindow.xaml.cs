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
    /// Interakční logika pro MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        MainWindow mw;
        public MenuWindow()
        {
            InitializeComponent();
            this.playerLabel.Content = GlobalConfig.CurrentPlayer.ToString();
            
            teamListView.ItemsSource = GlobalConfig.CurrentPlayer.Teams;
        }

        private void createTeamButton_Click(object sender, RoutedEventArgs e)
        {
            new createTeamWindow().Show();
            this.Close();
        }

        private void teamListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var team = teamListView.SelectedItem as TeamModel;
            if (team != null)
            {
                new teamWindow(team).Show();
                this.Close();
            }
        }

        private void joinTeamButton_Click(object sender, RoutedEventArgs e)
        {
            new joinTeamWindow().Show();
            this.Close();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.CurrentPlayer = null;
            new ChooseLoginWindow().Show();
            this.Close();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            new updatePlayerWindow().Show();
            this.Close();
        }
    }
}
