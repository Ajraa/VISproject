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
    /// Interakční logika pro updateFounderWindow.xaml
    /// </summary>
    public partial class updateFounderWindow : Window
    {
        public updateFounderWindow()
        {
            InitializeComponent();
            nameTextBox.Text = GlobalConfig.CurrentFounder.First_Name;
            lastNameTextBox.Text = GlobalConfig.CurrentFounder.Last_Name;
            nickNameTextBox.Text = GlobalConfig.CurrentFounder.Nickname;
            passwordTextBox.Text = GlobalConfig.CurrentFounder.Password;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new founderMenuWindow().Show();
            this.Close();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text != null)
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

            GlobalConfig.CurrentFounder.First_Name = nameTextBox.Text;
            GlobalConfig.CurrentFounder.Last_Name = lastNameTextBox.Text;
            GlobalConfig.CurrentFounder.Nickname = nickNameTextBox.Text;
            GlobalConfig.CurrentFounder.Password = passwordTextBox.Text;
            GlobalConfig.Connection.FounderDao.UpdateFounder();

            new founderMenuWindow().Show();
            this.Close();
        }
    }
}
