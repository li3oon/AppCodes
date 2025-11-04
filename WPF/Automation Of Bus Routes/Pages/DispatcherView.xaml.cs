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

namespace Automation_Of_Bus_Routes.Pages
{
    /// <summary>
    /// Логика взаимодействия для DispatcherView.xaml
    /// </summary>
    public partial class DispatcherView : Page
    {
        public DispatcherView()
        {
            InitializeComponent();
        }

        private void BtnVoyages_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDriverVoyage_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new DriverVoyageList(Manager.CurrentUser));
        }

        private void BtnVoyags_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new VoyagesList());
        }
    }
}
