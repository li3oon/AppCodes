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

namespace Bus_route_automation_system.Pages.Dispetcher
{
    /// <summary>
    /// Логика взаимодействия для Dispetcher.xaml
    /// </summary>
    public partial class Dispetcher : Page
    {
        private User _currentUser;
        public Dispetcher(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void BtnVoyages_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Pages.Voyages(_currentUser));
        }

        private void BtnDriverVoyages_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Pages.Dispetcher.DriverVoyages(_currentUser));
        }
    }
}
