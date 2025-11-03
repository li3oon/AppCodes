using System;
using System.Collections.Generic;
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

namespace Bus_route_automation_system.Pages.Dispetcher
{
    /// <summary>
    /// Логика взаимодействия для DriverVoyages.xaml
    /// </summary>
    public partial class DriverVoyages : Page
    {
        private User _currentUser;
        public DriverVoyages(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            DriverVoyagesGrid.ItemsSource = DataBase3.GetContext().Voyage.ToList();
            ConfigureUIBasedOnRole();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (_currentUser == null)
                return;
            if (_currentUser.Role == "Диспетчер")
            {
                // Для техника: показываем кнопки для добавления, редактирования и удаления услуг
                DriverVoyagesGrid.ItemsSource = DataBase3.GetContext().Voyage.ToList();
                BtnAdd.Visibility = Visibility.Visible;
                BtnDel.Visibility = Visibility.Visible;

            }
            else if (_currentUser.Role == "Водитель")
            {
                // Для Водителя: показываем только его услуги, отфильтровав по IdClient
                if (_currentUser.DriverID.HasValue)
                {
                    DriverVoyagesGrid.ItemsSource = DataBase3.GetContext().DriverVoyage
                        .Where(o => o.DriverID == _currentUser.DriverID.Value)
                        .ToList();
                }
                else
                {
                    DriverVoyagesGrid.ItemsSource = null;
                }

                BtnAdd.Visibility = Visibility.Collapsed;
                BtnDel.Visibility = Visibility.Collapsed;

                // Скрываем кнопку редактирования
                foreach (var column in DriverVoyagesGrid.Columns)
                {
                    if (column is DataGridTemplateColumn templateColumn)
                    {
                        if (templateColumn.Header != null && templateColumn.Header.ToString() == "Данные клиента")
                        {
                            templateColumn.Visibility = Visibility.Collapsed;
                        }

                        if (DriverVoyagesGrid.Columns.Count > 0)
                        {
                            DriverVoyagesGrid.Columns[DriverVoyagesGrid.Columns.Count - 1].Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
