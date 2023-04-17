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
    /// Interakční logika pro updatePlayerWindow.xaml
    /// </summary>
    public partial class updatePlayerWindow : Window
    {
        public updatePlayerWindow()
        {
            InitializeComponent();
            nameTextBox.Text = GlobalConfig.CurrentPlayer.First_Name;
            lastNameTextBox.Text = GlobalConfig.CurrentPlayer.Last_Name;
            nickNameTextBox.Text = GlobalConfig.CurrentPlayer.NickName;
            passwordTextBox.Text = GlobalConfig.CurrentPlayer.Password;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow().Show();
            this.Close();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text == null)
            {
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


            GlobalConfig.CurrentPlayer.First_Name = nameTextBox.Text;
            GlobalConfig.CurrentPlayer.Last_Name = lastNameTextBox.Text;
            GlobalConfig.CurrentPlayer.NickName = nickNameTextBox.Text;
            GlobalConfig.CurrentPlayer.Password = passwordTextBox.Text;

            GlobalConfig.Connection.PlayerDao.UpdatePlayer();
        }
    }
}
