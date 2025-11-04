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
    /// Логика взаимодействия для AddEditDriverVoyageList.xaml
    /// </summary>
    public partial class AddEditDriverVoyageList : Page
    {
        private DriverVoyage _currentDriverVoyage = new DriverVoyage();
        public AddEditDriverVoyageList(DriverVoyage currentDriverVoyage)
        {
            InitializeComponent();
            if (currentDriverVoyage != null)
                _currentDriverVoyage = currentDriverVoyage;
            DataContext = _currentDriverVoyage;

            ComboDriver.ItemsSource = DataBase3.GetContext().Driver.ToList();
            ComboVoyage.ItemsSource = DataBase3.GetContext().Voyage.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _currentDriverVoyage.Driver = ComboDriver.SelectedItem as Driver;
            _currentDriverVoyage.Voyage = ComboVoyage.SelectedItem as Voyage;

            StringBuilder errors = new StringBuilder();
            if (_currentDriverVoyage.Driver == null)
                errors.AppendLine("Укажите водителя");
            if (_currentDriverVoyage.Voyage == null)
                errors.AppendLine("Укажите рейс");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentDriverVoyage.ID == 0)
                DataBase3.GetContext().DriverVoyage.Add(_currentDriverVoyage);
            try
            {
                DataBase3.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
