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
using DataLibrary;
using DataLibrary.Model;

namespace project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerModel player = null;
            if (usernameBox.Text != null && passwordBox.Text != null)
            {
                player = GlobalConfig.Connection.PlayerDao.Login(usernameBox.Text, passwordBox.Text);
                if ( player != null)
                {
                    GlobalConfig.CurrentPlayer = player;
                    new MenuWindow().Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Uživatel neexistuje");
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            this.Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            new ChooseLoginWindow().Show();
            this.Close();
        }
    }
}
