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

namespace project
{
    /// <summary>
    /// Interakční logika pro ChooseLoginWindow.xaml
    /// </summary>
    public partial class ChooseLoginWindow : Window
    {
        public ChooseLoginWindow()
        {
            InitializeComponent();
        }

        private void playerButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void founderButton_Click(object sender, RoutedEventArgs e)
        {
            new founderLoginWindow().Show();
            this.Close();
        }
    }
}
