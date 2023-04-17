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
    /// Interakční logika pro founderLoginWindow.xaml
    /// </summary>
    public partial class founderLoginWindow : Window
    {
        public founderLoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            FounderModel founder = null;
            if (usernameBox.Text != null && passwordBox.Text != null)
            {
                founder = GlobalConfig.Connection.FounderDao.Login(usernameBox.Text, passwordBox.Text);
                if (founder != null)
                {
                    GlobalConfig.CurrentFounder = founder;
                    new founderMenuWindow().Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Uživatel neexistuje");
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            new founderRegisterWindow().Show();
            this.Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            new ChooseLoginWindow().Show();
            this.Close();
        }
    }
}
