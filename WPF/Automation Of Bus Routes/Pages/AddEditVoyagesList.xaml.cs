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

namespace Automation_Of_Bus_Routes.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditVoyagesList.xaml
    /// </summary>
    public partial class AddEditVoyagesList : Page
    {
        private Voyage _currentVoyage = new Voyage();

        public AddEditVoyagesList(Voyage currentVoyage)
        {
            InitializeComponent();

            if (currentVoyage != null)
                _currentVoyage = currentVoyage;
            DataContext = _currentVoyage;

            ComboRoute.ItemsSource = DataBase3.GetContext().Route.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный маршрут
            _currentVoyage.Route = ComboRoute.SelectedItem as Route;

            // Сбор ошибок валидации
            StringBuilder errors = new StringBuilder();

            // Подготовка значений из контролов (имена установлены в XAML)
            var companyText = NameCompanyBox?.Text;
            DateTime? dep = DepartureDatePicker?.SelectedDate;
            DateTime? arr = ArrivalDatePicker?.SelectedDate;

            // Проверки всех полей
            if (string.IsNullOrWhiteSpace(companyText))
                errors.AppendLine("Укажите компанию.");
            if (_currentVoyage.Route == null)
                errors.AppendLine("Укажите маршрут.");
            if (dep == null)
                errors.AppendLine("Укажите дату отправления.");
            if (arr == null)
                errors.AppendLine("Укажите дату прибытия.");
            if (dep != null && arr != null && dep > arr)
                errors.AppendLine("Дата отправления не может быть позже даты прибытия.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Присваиваем связанные значения в модель
            if (_currentVoyage.Route != null)
                _currentVoyage.RouteID = _currentVoyage.Route.ID;
            _currentVoyage.DepartureDate = dep.Value;
            _currentVoyage.ArrivalDate = arr.Value;

            if (_currentVoyage.ID == 0)
                DataBase3.GetContext().Voyage.Add(_currentVoyage);
            try
            {
                DataBase3.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }   
        }
    }
}
