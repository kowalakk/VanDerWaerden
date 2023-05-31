using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for GameSettings.xaml
    /// </summary>
    public partial class GameSettings : Window
    {
        Configuration GameConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        string[] algorithms = new string[] { "UCT", "MinMax", "Random" };
        public GameSettings()
        {
            InitializeComponent();

            Player1ComboBox.ItemsSource = algorithms;
            Player2ComboBox.ItemsSource = algorithms;

            if (GameConfiguration.Sections["VanDerWaerdenGameSettings"] is null)
            {
                GameConfiguration.Sections.Add("VanDerWaerdenGameSettings", new VanDerWaerdenGameSettings());
            }

            var GameSettingSection = GameConfiguration.GetSection("VanDerWaerdenGameSettings");

            this.DataContext = GameSettingSection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameConfiguration.Save();
            this.Close();
        }
    }
}
