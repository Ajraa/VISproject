using DataLibrary;
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
    /// Interaction logic for XMLGeneratorWindow.xaml
    /// </summary>
    public partial class XMLGeneratorWindow : Window
    {
        public XMLGeneratorWindow()
        {
            InitializeComponent();
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            int playerID = -1;
            int? teamID = null;

            int.TryParse(this.playerID.Text, out playerID);
            if (int.TryParse(this.teamID.Text, out int parsedValue))
                teamID = parsedValue;
            string xml = GlobalConfig.Connection.PlayerDao.GenerateXml(playerID, teamID);
            this.xmlText.Text = xml;
        }
    }
}
