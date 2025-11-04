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

namespace Automation_Of_Bus_Routes.Pages
{
    /// <summary>
    /// Логика взаимодействия для VoyagesList.xaml
    /// </summary>
    public partial class VoyagesList : Page
    {
        public VoyagesList()
        {
            InitializeComponent();
            VoyageGrid.ItemsSource = DataBase3.GetContext().Voyage.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditVoyagesList());
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
