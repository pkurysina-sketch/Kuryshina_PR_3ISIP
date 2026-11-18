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

namespace CarConfigurator.Services
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            ComboModel.ItemsSource = new List<string>
                { "Sedan Basic (1 000 000)", "SUV Premium (2 500 000)" };
            ComboModel.SelectedIndex = 0;
        }

        private void ComboModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboEngine.ItemsSource = null;
            ComboColor.ItemsSource = null;

            if (ComboModel.SelectedIndex == 0)
            {
                ComboEngine.ItemsSource = new List<string> { "1.6 MPI (0)", "2.0 TSI (+150 000)" };
                ComboColor.ItemsSource = new List<string> { "Белый (0)", "Чёрный (+20 000)" };
                ConfiguratorData.BasePrice = 1_000_000m;
            }
            else
            {
                ComboEngine.ItemsSource = new List<string> { "2.0 TSI (0)", "3.0 V6 (+300 000)" };
                ComboColor.ItemsSource = new List<string> { "Серебристый (0)", "Синий (+40 000)" };
                ConfiguratorData.BasePrice = 2_500_000m;
            }
            ComboEngine.SelectedIndex = 0;
            ComboColor.SelectedIndex = 0;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            // сохраняем выбранное
            ConfiguratorData.ModelName = ComboModel.SelectedItem as string;
            ConfiguratorData.EnginePrice = ComboEngine.SelectedIndex == 1 ? 150_000m : 0m;
            ConfiguratorData.ColorPrice = ComboColor.SelectedIndex == 1 ? 20_000m : 0m;
            ConfiguratorData.OptionsPrice = 0;
            if (ChkLeather.IsChecked == true) ConfiguratorData.OptionsPrice += 80_000m;
            if (ChkPanorama.IsChecked == true) ConfiguratorData.OptionsPrice += 60_000m;

            // переход
            NavigationService.Navigate(new Page2());
        }
    }
}

