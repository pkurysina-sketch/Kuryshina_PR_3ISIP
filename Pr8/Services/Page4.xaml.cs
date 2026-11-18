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

    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();
            SummaryText.Text = $"{ConfiguratorData.ModelName}" + $"Итого: {ConfiguratorData.TotalPrice:N0} ₽\n" + $"Взнос: {ConfiguratorData.DownPayment:N0} ₽\n" +  $"Срок: {ConfiguratorData.TermMonths} мес.\n" + $"Платёж: {ConfiguratorData.MonthlyPayment:N0} ₽";
        }
        private void CheckValidation(object sender, TextChangedEventArgs e)
        {
            bool ok =
                TxtName.Text.Trim().Length >= 2 &&
                TxtPhone.Text.Length > 0 && TxtPhone.Text.All(char.IsDigit) &&
                TxtEmail.Text.Contains("@");

            BtnSubmit.IsEnabled = ok;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            ConfiguratorData.FullName = TxtName.Text;
            ConfiguratorData.Phone = TxtPhone.Text;
            ConfiguratorData.Email = TxtEmail.Text;
            MessageBox.Show("Заявка успешно отправлена!");
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (!BtnSubmit.IsEnabled)
            {
                var r = MessageBox.Show(
                    "Контакты не заполнены. Выйти без сохранения?",
                    "Внимание",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (r == MessageBoxResult.No) return;
            }
            NavigationService.GoBack();
        }
    }
}

