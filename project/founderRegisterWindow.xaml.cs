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
    /// Interakční logika pro founderRegisterWindow.xaml
    /// </summary>
    public partial class founderRegisterWindow : Window
    {
        public founderRegisterWindow()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
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

            FounderModel fm = new FounderModel();
            fm.First_Name = nameTextBox.Text;
            fm.Last_Name = lastNameTextBox.Text;
            fm.Nickname = nickNameTextBox.Text;
            fm.Password = passwordTextBox.Text;
            GlobalConfig.Connection.FounderDao.CreateFounder(fm);
            GlobalConfig.CurrentFounder = fm;

            new founderMenuWindow().Show();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            new founderLoginWindow().Show();
            this.Close();
        }
    }
}
