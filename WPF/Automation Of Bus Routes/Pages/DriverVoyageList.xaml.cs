using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
    /// Логика взаимодействия для DriverVoyageList.xaml
    /// </summary>
    public partial class DriverVoyageList : Page
    {
        private User _currentUser;
        private List<DriverVoyage> _allDriverVoyages;
        public DriverVoyageList(User currentUser)
        {
            InitializeComponent();
            DriverVoyageGrid.ItemsSource = DataBase3.GetContext()
                .DriverVoyage
                .Include(dv => dv.Voyage.Route)
                .ToList();
            _currentUser = currentUser;
            ConfigureUIBasedOnRole();
            LoadData();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (_currentUser == null)
                return;

            if (_currentUser.Role == "Диспетчер")
            {

                DriverVoyageGrid.ItemsSource = DataBase3.GetContext().DriverVoyage
                    .Include(dv => dv.Voyage.Route)
                    .ToList();
                BtnAdd.Visibility = Visibility.Visible;
                BtnDel.Visibility = Visibility.Visible;

            }
            else if (_currentUser.Role == "Водитель")
            {
                
                if (_currentUser.DriverID.HasValue)
                {
                    DriverVoyageGrid.ItemsSource = DataBase3.GetContext().DriverVoyage
                        .Include(dv => dv.Voyage.Route)
                        .Where(o => o.DriverID == _currentUser.DriverID.Value)
                        .ToList();
                }
                else
                {
                    DriverVoyageGrid.ItemsSource = null;
                }
                
                BtnAdd.Visibility = Visibility.Collapsed;
                BtnDel.Visibility = Visibility.Collapsed;
                txtfilter1.Visibility = Visibility.Collapsed;

                foreach (var column in DriverVoyageGrid.Columns)
                {
                    if (column is DataGridTemplateColumn templateColumn)
                    {
                        if (DriverVoyageGrid.Columns.Count > 0)
                        {
                            DriverVoyageGrid.Columns[DriverVoyageGrid.Columns.Count - 1].Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void LoadData()
        {
            // Загружаем все записи и навигационные свойства, которыми будем фильтровать
            _allDriverVoyages = DataBase3.GetContext().DriverVoyage
                .Include(dv => dv.Voyage.Route)
                .Include(dv => dv.Driver)
                .ToList();

            DriverVoyageGrid.ItemsSource = _allDriverVoyages;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditDriverVoyageList());
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (end.SelectedDate == null || end.SelectedDate < start.SelectedDate)
            {
                end.SelectedDate = DateTime.Now.Date;
            }

            UpdateFilters();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFilters();
        }

        private void txtFilter(object sender, TextChangedEventArgs e)
        {
            // Явно вызываем обновление фильтра при изменении текста
            UpdateFilters();
        }

        // Подробный и читаемый метод фильтрации: поиск только по ФИО водителя и по диапазону дат отправления
        private void UpdateFilters()
        {
            // Получаем представление коллекции, привязанной к DataGrid
            ICollectionView view = CollectionViewSource.GetDefaultView(DriverVoyageGrid.ItemsSource);

            // Если источника данных нет — нечего фильтровать
            if (view == null)
            {
                return;
            }

            // Берём текст фильтра из TextBox и нормализуем
            string rawText = string.Empty;
            if (txtfilter1 != null && txtfilter1.Text != null)
            {
                rawText = txtfilter1.Text.Trim();
            }

            string filterText = rawText.ToLowerInvariant();

            // Получаем границы диапазона дат (если заданы)
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (start != null && start.SelectedDate.HasValue)
            {
                startDate = start.SelectedDate.Value.Date;
            }

            if (end != null && end.SelectedDate.HasValue)
            {
                // Устанавливаем конец дня для корректного сравнения по времени
                endDate = end.SelectedDate.Value.Date;
                endDate = endDate.Value.AddDays(1).AddTicks(-1);
            }

            // Устанавливаем фильтр: объект проходит, если удовлетворяет всем активным критериям
            view.Filter = obj =>
            {
                // Проверяем тип
                if (obj == null)
                {
                    return false;
                }

                DriverVoyage dv = obj as DriverVoyage;
                if (dv == null)
                {
                    return false;
                }

                // Фильтрация по дате отправления (если обе даты заданы)
                if (startDate.HasValue && endDate.HasValue)
                {
                    Voyage voyage = dv.Voyage;
                    if (voyage == null)
                    {
                        return false;
                    }

                    DateTime departure = voyage.DepartureDate;
                    if (departure < startDate.Value)
                    {
                        return false;
                    }

                    if (departure > endDate.Value)
                    {
                        return false;
                    }
                }

                // Если текстового фильтра нет — элемент проходит
                if (string.IsNullOrEmpty(filterText))
                {
                    return true;
                }

                // Поиск только по ФИО водителя
                Driver driver = dv.Driver;
                if (driver == null)
                {
                    return false;
                }

                string last = driver.LastName ?? string.Empty;
                string first = driver.FirstName ?? string.Empty;
                string patronymic = driver.Patronymic ?? string.Empty;

                string fullName = string.Concat(last, " ", first, " ", patronymic).ToLowerInvariant();

                if (fullName.Contains(filterText))
                {
                    return true;
                }

                return false;
            };
        }
    }
}
