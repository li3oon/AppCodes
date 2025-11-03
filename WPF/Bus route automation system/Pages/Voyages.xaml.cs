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

namespace Bus_route_automation_system.Pages
{
    /// <summary>
    /// Логика взаимодействия для Voyages.xaml
    /// </summary>
    public partial class Voyages : Page
    {
        private User _currentUser;
        public Voyages(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            VoyagesGrid.ItemsSource = DataBase3.GetContext().Voyage.ToList();
        }

        
        

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
