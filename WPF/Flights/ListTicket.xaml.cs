using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
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

namespace Flights
{
    /// <summary>
    /// Логика взаимодействия для ListTicket.xaml
    /// </summary>
    public partial class ListTicket : Page
    {
        private User _currentUser;
        public ListTicket(User currentUser)
        {
            InitializeComponent();
            TicketGrid.ItemsSource = DataBase.GetContext().Ticket.ToList();

            _currentUser = currentUser;

            ConfigureUIBasedOnRole();
            LoadDirections();
            LoadTickets();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (_currentUser == null)
                return;

            if (_currentUser.CustomerID.HasValue)
            {
                TicketGrid.ItemsSource = DataBase.GetContext().Ticket
                    .Include(t => t.Trip)
                        .Include(tr => tr.Trip.Direction)
                    .Include(t => t.Customer)
                    .Where(t => t.CustomerID == _currentUser.CustomerID.Value)
                    .ToList();
            }
            else
            {
                TicketGrid.ItemsSource = null;
            }
        }

        private void LoadDirections()
        {
            var context = DataBase.GetContext();

            var directions = context.Direction
                .Select(d => new
                {
                    d.ID,
                    RouteName = d.DeparturePoint + " - " + d.ArrivalPoint
                })
                .ToList();

            // добавляем пункт "Все маршруты" для удобства
            filter.ItemsSource = directions;
            filter.SelectedIndex = -1;
        }

        private void LoadTickets(int? directionId = null)
        {
            var context = DataBase.GetContext();

            var ticketsQuery = context.Ticket
                .Include(t => t.Customer)
                .Include(t => t.Trip)
                    .Include(tr => tr.Trip.Direction)
                .AsQueryable();

            // Если это покупатель — ограничиваем по CustomerID
            if (_currentUser.CustomerID.HasValue)
            {
                ticketsQuery = ticketsQuery
                    .Where(t => t.CustomerID == _currentUser.CustomerID.Value);
            }

            if (directionId.HasValue)
            {
                ticketsQuery = ticketsQuery
                    .Where(t => t.Trip.DirectionID == directionId.Value);
            }

            TicketGrid.ItemsSource = ticketsQuery.ToList();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter.SelectedValue is int directionId)
                LoadTickets(directionId);
            else
                LoadTickets(); // если ничего не выбрано — показать всё
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

            // Получаем исходный список билетов из БД
            var context = DataBase.GetContext();

            var ticketsQuery = context.Ticket
                .Include(t => t.Trip)
                    .Include(tr => tr.Trip.Direction)
                .Include(t => t.Customer)
                .AsQueryable();

            if (_currentUser.CustomerID.HasValue)
            {
                ticketsQuery = ticketsQuery
                    .Where(t => t.CustomerID == _currentUser.CustomerID.Value);
            }

            // Фильтрация по диапазону дат
            ticketsQuery = ticketsQuery
                .Where(t => t.Trip.DepartureDate >= startDate && t.Trip.DepartureDate <= endDate);

            TicketGrid.ItemsSource = ticketsQuery.ToList();
        }
    }
}
