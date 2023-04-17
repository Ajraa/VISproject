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
    /// Interakční logika pro createTournamentWindow.xaml
    /// </summary>
    public partial class createTournamentWindow : Window
    {
        public createTournamentWindow()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new founderMenuWindow().Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text == null) { 
                MessageBox.Show("Zadejte jméno");
                return;
            }

            TournamentModel tm = new TournamentModel();
            tm.Founder = GlobalConfig.CurrentFounder;
            tm.Name = nameTextBox.Text;
            tm.Price = int.Parse(priceTextBox.Text);
            tm.Prize = int.Parse(prizeTextBox.Text);
            tm.Min_Rating = int.Parse(minRatingTextBox.Text);
            tm.Max_Rating = int.Parse(maxRatingTextBox.Text);
            GlobalConfig.CurrentFounder.TournamentList.Add(GlobalConfig.Connection.TournamentDao.CreateTournament(tm));
            
            new founderMenuWindow().Show();
            this.Close();
        }
    }
}
