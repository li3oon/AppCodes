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

namespace Network_of_Technical_Inspection_Stations
{
    /// <summary>
    /// Логика взаимодействия для ClientService.xaml
    /// </summary>
    public partial class ClientService : Page
    {
        private User CurrentUser;
        public ClientService(User currentUser)
        {
            InitializeComponent();
            ClientServiceGrid.ItemsSource = DataBase.GetContext().Order.ToList();

            CurrentUser = currentUser;
            ConfigureUIBasedOnRole();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (CurrentUser == null)
                return;

            if (CurrentUser.Role == "Техник")
            {
                // Для техника: показываем кнопки для добавления, редактирования и удаления услуг
                ClientServiceGrid.ItemsSource = DataBase.GetContext().Order.ToList();
                BtnAddClientService.Visibility = Visibility.Visible;
                BtnDelClientService.Visibility = Visibility.Visible;

            }
            else if (CurrentUser.Role == "Клиент")
            {
                // Для клиента: показываем только его услуги, отфильтровав по IdClient
                if (CurrentUser.IdClient.HasValue)
                {
                    ClientServiceGrid.ItemsSource = DataBase.GetContext().Order
                        .Where(o => o.IdClient == CurrentUser.IdClient.Value)
                        .ToList();
                }
                else
                {
                    ClientServiceGrid.ItemsSource = null;
                }

                // Для клиента: скрываем кнопки редактирования и удаления, только просмотр
                BtnAddClientService.Visibility = Visibility.Collapsed;
                BtnDelClientService.Visibility = Visibility.Collapsed;

                // Скрываем кнопку редактирования
                foreach (var column in ClientServiceGrid.Columns)
                {
                    if (column is DataGridTemplateColumn templateColumn)
                    {
                        if (templateColumn.Header != null && templateColumn.Header.ToString() == "Данные клиента")
                        {
                            templateColumn.Visibility = Visibility.Collapsed;
                        }

                        if (ClientServiceGrid.Columns.Count > 0)
                        {
                            ClientServiceGrid.Columns[ClientServiceGrid.Columns.Count - 1].Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }
        private void BtnAddClientService_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditClientService(null));
        }

        private void BtnDelClientService_Click(object sender, RoutedEventArgs e)
        {
            var ClientForRemoving = ClientServiceGrid.SelectedItems.Cast<Order>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {ClientForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DataBase.GetContext().Order.RemoveRange(ClientForRemoving);
                    DataBase.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    ClientServiceGrid.ItemsSource = DataBase.GetContext().Order.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditClientService((sender as Button).DataContext as Order));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                ClientServiceGrid.ItemsSource = DataBase.GetContext().Order.ToList();
                DataBase.GetContext().ChangeTracker.Entries()
                .Where(p => p.State != EntityState.Added)
                .ToList()
                .ForEach(p => p.Reload());
                ConfigureUIBasedOnRole();
            }
        }
    }
}
