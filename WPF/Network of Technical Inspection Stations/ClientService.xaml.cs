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
    /// Логика взаимодействия для ClientService.xaml
    /// </summary>
    public partial class ClientService : Page
    {
        public ClientService()
        {
            InitializeComponent();
            ClientServiceGrid.ItemsSource = DataBase.GetContext().Order.ToList();
        }

        private void BtnAddClientService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelClientService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
