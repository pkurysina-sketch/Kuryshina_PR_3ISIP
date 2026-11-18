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
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            ConfiguratorData.TotalPrice =
        ConfiguratorData.BasePrice +
        ConfiguratorData.EnginePrice +
        ConfiguratorData.ColorPrice +
        ConfiguratorData.OptionsPrice;

            PriceText.Text = $"{ConfiguratorData.TotalPrice:N0} ₽";
            DetailsText.Text =
                $"Модель: {ConfiguratorData.ModelName}\n" +
                $"Двигатель: +{ConfiguratorData.EnginePrice:N0} ₽\n" +
                $"Цвет: +{ConfiguratorData.ColorPrice:N0} ₽\n" +
                $"Опции: +{ConfiguratorData.OptionsPrice:N0} ₽";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) =>
            NavigationService.GoBack();

        private void BtnNext_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new Page3());
    }
}

