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
            CurrentUser = currentUser;

            // Проверка роли и отображение/скрытие элементов управления
            ConfigureUIBasedOnRole();
        }

        private void ConfigureUIBasedOnRole()
        {
            // Проверка роли пользователя
            if (CurrentUser.Role == "Техник")
            {
                // Для техника: показываем кнопки для добавления, редактирования и удаления услуг
                BtnAddService.Visibility = Visibility.Visible;
            }
            else if (CurrentUser.Role == "Клиент")
            {
                // Для клиента: скрываем кнопки редактирования и удаления, только просмотр
                BtnAddService.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

