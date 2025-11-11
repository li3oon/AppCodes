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
using System.Data.Entity;

namespace Flights
{
    /// <summary>
    /// Логика взаимодействия для ListTrips.xaml
    /// </summary>
    public partial class ListTrips : Page
    {
        private User _currentUser;
        public ListTrips(User currentUser)
        {
            InitializeComponent();
            TripGrid.ItemsSource = DataBase.GetContext().Trip.ToList();

            _currentUser = currentUser;
            ConfigureUIBasedOnRole();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (_currentUser == null)
                return;

            if (_currentUser.Role == "Кассир")
            {
                BtnAdd.Visibility = Visibility.Visible;
                BtnDel.Visibility = Visibility.Visible;

            }
            else if (_currentUser.Role == "Покупатель")
            {

                BtnAdd.Visibility = Visibility.Collapsed;
                BtnDel.Visibility = Visibility.Collapsed;

                foreach (var column in TripGrid.Columns)
                {
                    if (column is DataGridTemplateColumn templateColumn)
                    {
                        if (TripGrid.Columns.Count > 0)
                        {
                            TripGrid.Columns[TripGrid.Columns.Count - 1].Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditListTrips(null)); 
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var tripsForRemoving = TripGrid.SelectedItems.Cast<Trip>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {tripsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DataBase.GetContext().Trip.RemoveRange(tripsForRemoving);
                    DataBase.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    TripGrid.ItemsSource = DataBase.GetContext().Trip.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditListTrips((sender as Button).DataContext as Trip));
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (end.SelectedDate == null || end.SelectedDate < start.SelectedDate)
            {
                end.SelectedDate = DateTime.Now.Date;
            }

            ApplyDateFilter();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyDateFilter();
        }

        private void ApplyDateFilter()
        {
            if (start.SelectedDate == null || end.SelectedDate == null)
                return;

            DateTime startDate = start.SelectedDate.Value.Date;
            DateTime endDate = end.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);

            using (var context = DataBase.GetContext())
            {
                var tripsQuery = context.Trip
                    .Include(tr => tr.Direction)
                    .AsQueryable();

                // фильтрация по диапазону дат
                tripsQuery = tripsQuery
                    .Where(t => t.DepartureDate.Date >= startDate && t.DepartureDate.Date <= endDate);

                TripGrid.ItemsSource = tripsQuery.ToList();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DataBase.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                TripGrid.ItemsSource = DataBase.GetContext().Trip.ToList();
            }
        }
    }
}
