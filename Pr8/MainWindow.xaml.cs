using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Car
{
    public partial class MainWindow : Window
    {
        private int currentStep = 1;
        private decimal basePrice = 0;
        private decimal enginePrice = 0;
        private decimal colorPrice = 0;
        private decimal totalPrice = 0;
        private decimal downPayment = 0;
        private decimal monthlyPayment = 0;

        public MainWindow()
        {
            InitializeComponent();

            ComboModel.ItemsSource = new List<string> { "Sedan Basic (1 000 000)", "SUV Premium (2 500 000)" };
            ComboModel.SelectedIndex = 0;

            UpdateStep();
        }


        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep == 4)
            {
                MessageBox.Show("Заявка успешно отправлена!");
                return;
            }

            currentStep++;

            if (currentStep == 2) CalculatePrice();
            if (currentStep == 3) CalculateLoan();
            if (currentStep == 4) UpdateSummary();

            UpdateStep();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на потерю данных при уходе с шага заявки
            if (currentStep == 4 && !BtnSubmit.IsEnabled)
            {
                var result = MessageBox.Show("Контакты не заполнены. Выйти без сохранения?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No) return;
            }

            currentStep--;
            UpdateStep();
        }

        private void UpdateStep()
        {
            Step1Panel.Visibility = Visibility.Collapsed;
            Step2Panel.Visibility = Visibility.Collapsed;
            Step3Panel.Visibility = Visibility.Collapsed;
            Step4Panel.Visibility = Visibility.Collapsed;

            if (currentStep == 1) Step1Panel.Visibility = Visibility.Visible;
            if (currentStep == 2) Step2Panel.Visibility = Visibility.Visible;
            if (currentStep == 3) Step3Panel.Visibility = Visibility.Visible;
            if (currentStep == 4) Step4Panel.Visibility = Visibility.Visible;

            StepText.Text = $"Шаг {currentStep} из 4";
            Progress.Value = currentStep;
        }


        private void ComboModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboModel.SelectedIndex == 0) // Sedan
            {
                ComboEngine.ItemsSource = new List<string> { "1.6 Бензин (+0)", "2.0 Бензин (+150 000)" };
                ComboColor.ItemsSource = new List<string> { "Белый (+0)", "Черный металлик (+25 000)" };
            }
            else // SUV
            {
                ComboEngine.ItemsSource = new List<string> { "2.0 Турбо (+0)", "3.0 V6 (+400 000)" };
                ComboColor.ItemsSource = new List<string> { "Синий (+30 000)", "Красный (+50 000)" };
            }

            ComboEngine.SelectedIndex = 0;
            ComboColor.SelectedIndex = 0;
        }



        private void CalculatePrice()
        {
            // 1. Базовая цена
            basePrice = ComboModel.SelectedIndex == 0 ? 1000000 : 2500000;

            // 2. Двигатель
            if (ComboEngine.SelectedIndex == 0) enginePrice = 0;
            else enginePrice = ComboModel.SelectedIndex == 0 ? 150000 : 400000;

            // 3. Цвет
            if (ComboColor.SelectedIndex == 0) colorPrice = 0;
            else colorPrice = ComboModel.SelectedIndex == 0 ? 25000 : (ComboColor.SelectedIndex == 1 ? 30000 : 50000);

            // 4. Опции
            decimal optionsPrice = 0;
            if (ChkLeather.IsChecked == true) optionsPrice += 80000;
            if (ChkPanorama.IsChecked == true) optionsPrice += 60000;

            // 5. Итог
            totalPrice = basePrice + enginePrice + colorPrice + optionsPrice;

            // Вывод на экран
            PriceText.Text = $"{totalPrice:N0} руб.";
            DetailsText.Text = $"База: {basePrice:N0} руб.\n" +
                               $"Двигатель: {enginePrice:N0} руб.\n" +
                               $"Цвет: {colorPrice:N0} руб.\n" +
                               $"Опции: {optionsPrice:N0} руб.";
        }

        private void CalculateLoan()
        {
            double percent = SliderDownPayment.Value;
            int months = (int)SliderTerm.Value;

            DownPaymentLabel.Text = $"Первоначальный взнос: {percent}%";
            TermLabel.Text = $"Срок: {months} мес.";

            downPayment = totalPrice * (decimal)(percent / 100);
            decimal loanBody = totalPrice - downPayment;
            monthlyPayment = loanBody / months;

            LoanResultText.Text = $"Сумма взноса: {downPayment:N0} руб.\n" +
                                  $"Сумма кредита: {loanBody:N0} руб.\n" +
                                  $"Платеж в месяц: {monthlyPayment:N0} руб.";
        }

        private void LoanChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded && currentStep == 3)
            {
                CalculateLoan();
            }
        }


        private void CheckValidation(object sender, TextChangedEventArgs e)
        {
            bool isValid = TxtName.Text.Length >= 2 &&
                           TxtPhone.Text.Length > 0 && TxtPhone.Text.All(char.IsDigit) &&
                           TxtEmail.Text.Contains("@");

            BtnSubmit.IsEnabled = isValid;
        }

        private void UpdateSummary()
        {
            SummaryText.Text = $"Автомобиль: {ComboModel.SelectedItem}\n" +
                               $"Стоимость: {totalPrice:N0} руб.\n" +
                               $"Платеж: {monthlyPayment:N0} руб./мес.";
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Заявка от {TxtName.Text} принята!\nМы свяжемся с вами по телефону {TxtPhone.Text}", "Успех");
        }
    }
}

