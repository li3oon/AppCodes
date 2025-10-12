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

namespace Network_of_Technical_Inspection_Stations
{
    /// <summary>
    /// Логика взаимодействия для Services.xaml
    /// </summary>
    public partial class Services : Page
    {
        private User CurrentUser; // Текущий пользователь

        public Services(User currentUser)
        {
            InitializeComponent();
            ServicesGrid.ItemsSource = DataBase.GetContext().Service.ToList();
            CurrentUser = currentUser;

            // Проверка роли и отображение/скрытие элементов управления
            ConfigureUIBasedOnRole();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (CurrentUser == null)
                return;

            if (CurrentUser.Role == "Техник")
            {
                // Для техника: показываем кнопки для добавления, редактирования и удаления услуг
                BtnAddService.Visibility = Visibility.Visible;
                BtnDelService.Visibility = Visibility.Visible;

            }
            else if (CurrentUser.Role == "Клиент")
            {
                // Для клиента: скрываем кнопки редактирования и удаления, только просмотр
                BtnAddService.Visibility = Visibility.Collapsed;
                BtnDelService.Visibility = Visibility.Collapsed;

                // Скрываем кнопку редактирования
                foreach (var column in ServicesGrid.Columns)
                {
                    if (column is DataGridTemplateColumn templateColumn)
                    {
                        if (templateColumn.Header == null)
                        {
                            templateColumn.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditService(null));
        }

        private void BtnDelService_Click(object sender, RoutedEventArgs e)
        {
            var servicesForRemoving = ServicesGrid.SelectedItems.Cast<Service>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {servicesForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var service in servicesForRemoving)
                    {
                        service.IsDeleted = true;
                    }
                    DataBase.GetContext().SaveChanges();
                    MessageBox.Show("Услуги помечены как удалённые!");
                    // Обновить отображение, исключая удалённые услуги
                    ServicesGrid.ItemsSource = DataBase.GetContext().Service.Where(s => !s.IsDeleted).ToList();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //(sender as Button).DataContext as Service
            Manager.MainFrame.Navigate(new AddEditService((sender as Button).DataContext as Service));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                ConfigureUIBasedOnRole();

                DataBase.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ServicesGrid.ItemsSource = DataBase.GetContext().Service.ToList();
            }
        }
    }
}

