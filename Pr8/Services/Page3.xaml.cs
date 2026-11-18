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
    public partial class Page3 : Page
    {
        private const double RatePercent = 12.0;
        public Page3()
        {
            InitializeComponent();
            UpdateLabels();
        }

        private void LoanChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LoanResultText == null) return;
            if (SliderTerm == null || SliderDownPayment == null) return;
            if (ConfiguratorData.TotalPrice <= 0) return;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            double dp = SliderDownPayment.Value;
            int term = (int)SliderTerm.Value;

            DownPaymentLabel.Text = $"Первоначальный взнос: {dp:0}%";
            TermLabel.Text = $"Срок (мес): {term}";

            decimal down = ConfiguratorData.TotalPrice * (decimal)dp / 100m;
            decimal credit = ConfiguratorData.TotalPrice - down;

            double monthRate = RatePercent / 100.0 / 12.0;
            double monthly = credit == 0
                ? 0
                : (double)credit * monthRate /
                  (1 - Math.Pow(1 + monthRate, -term));

            ConfiguratorData.DownPayment = down;
            ConfiguratorData.MonthlyPayment = (decimal)monthly;
            ConfiguratorData.TermMonths = term;

            LoanResultText.Text =
                $"Взнос: {down:N0} ₽\n" +
                $"Кредит: {credit:N0} ₽\n" +
                $"Платёж в месяц: {monthly:N0} ₽";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) =>
            NavigationService.GoBack();

        private void BtnNext_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new Page4());

    }
}

