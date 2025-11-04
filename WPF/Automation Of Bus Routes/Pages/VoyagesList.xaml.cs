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
            Manager.MainFrame.Navigate(new AddEditVoyagesList((sender as Button).DataContext as Voyage));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditVoyagesList(null));
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var voyagesForRemoving = VoyageGrid.SelectedItems.Cast<Voyage>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {voyagesForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DataBase3.GetContext().Voyage.RemoveRange(voyagesForRemoving);
                    DataBase3.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    VoyageGrid.ItemsSource = DataBase3.GetContext().Voyage.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DataBase3.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                VoyageGrid.ItemsSource = DataBase3.GetContext().Voyage.ToList();
            }
        }
    }
}
