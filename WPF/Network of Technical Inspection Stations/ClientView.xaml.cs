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
    /// Логика взаимодействия для ClientView.xaml
    /// </summary>
    public partial class ClientView : Page
    {
        private User _currentUser;
        public ClientView(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void BtnService1_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Services(_currentUser));
        }

        private void BtnClientService1_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ClientService(_currentUser));
        }
    }
}
