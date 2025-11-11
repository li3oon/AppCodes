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

namespace Flights
{
    /// <summary>
    /// Логика взаимодействия для AddEditListTrips.xaml
    /// </summary>
    public partial class AddEditListTrips : Page
    {
        private Trip _currentTrip = new Trip();
        public AddEditListTrips(Trip currentTrip)
        {
            InitializeComponent();

            if (currentTrip != null)
                _currentTrip = currentTrip;
            DataContext = _currentTrip;

            ComboDirection.ItemsSource = DataBase.GetContext().Direction.ToList();

            if (_currentTrip.DirectionID != 0)
                ComboDirection.SelectedItem = DataBase.GetContext().Direction.ToList().FirstOrDefault(d => d.ID == _currentTrip.DirectionID);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            // Получаем выбранный маршрут
            _currentTrip.Direction = ComboDirection.SelectedItem as Direction;

            // Сбор ошибок валидации
            StringBuilder errors = new StringBuilder();

            // Значения из контролов
            DateTime? dep = DepartureDatePicker?.SelectedDate;
            DateTime? arr = ArrivalDatePicker?.SelectedDate;
            string airplaneText = NameAirplane?.Text;

            // Проверки всех полей
            if (_currentTrip.Direction == null)
                errors.AppendLine("Укажите маршрут.");
            if (dep == null)
                errors.AppendLine("Укажите дату отправления.");
            if (arr == null)
                errors.AppendLine("Укажите дату прибытия.");
            if (dep != null && arr != null && dep > arr)
                errors.AppendLine("Дата отправления не может быть позже даты прибытия.");
            if (string.IsNullOrWhiteSpace(airplaneText))
                errors.AppendLine("Укажите название самолета.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Присваиваем связанные значения
            if (_currentTrip.Direction != null)
                _currentTrip.DirectionID = _currentTrip.Direction.ID;

            _currentTrip.DepartureDate = dep.Value;
            _currentTrip.ArrivalDate = arr.Value;
            _currentTrip.Airplane = airplaneText;

            var context = DataBase.GetContext();

            if (_currentTrip.ID == 0)
                context.Trip.Add(_currentTrip);

            try
            {
                context.SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
