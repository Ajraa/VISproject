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
    /// Interakční logika pro createTeamWindow.xaml
    /// </summary>
    public partial class createTeamWindow : Window
    {
        public createTeamWindow()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow().Show();
            this.Close();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text == null) 
            {
                MessageBox.Show("Zadejte jméno týmu");
                return;
            }
            if (tagTextBox.Text == null)
            {
                MessageBox.Show("Zadejte tag týmu");
                return;
            }
            if (tagTextBox.Text.Length != 3)
            {
                MessageBox.Show("Tag musí mít 3 znaky");
                return;
            }

            TeamModel tm = new TeamModel();
            tm.Team_Name = nameTextBox.Text;
            tm.Tag = tagTextBox.Text;
            tm.Captain = GlobalConfig.CurrentPlayer;
            tm.Rating = GlobalConfig.CurrentPlayer.Rating;
            GlobalConfig.Connection.TeamDao.CreateTeam(tm);

            new MenuWindow().Show();
            this.Close();
        }
    }
}
