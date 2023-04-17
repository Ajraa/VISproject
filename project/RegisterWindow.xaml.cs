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
using DataLibrary.Model;
using DataLibrary;

namespace project
{
    /// <summary>
    /// Interakční logika pro RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text == null) {
                MessageBox.Show("Vyplňte jméno");
                return;
            }
            if (lastNameTextBox.Text == null)
            {
                MessageBox.Show("Vyplňte příjmení");
                return;
            }
            if (nickNameTextBox.Text == null)
            {
                MessageBox.Show("Vyplňte username");
                return;
            }
            if (passwordTextBox.Text == null)
            {
                MessageBox.Show("Vyplňte heslo");
                return;
            }

            PlayerModel pm = new PlayerModel();
            pm.First_Name = nameTextBox.Text;
            pm.Last_Name = lastNameTextBox.Text;
            pm.NickName = nickNameTextBox.Text;
            pm.Password = passwordTextBox.Text;
            pm.Rating = 1000;
            GlobalConfig.Connection.PlayerDao.CreatePlayer(pm);
            GlobalConfig.CurrentPlayer = pm;

            new MenuWindow().Show();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
