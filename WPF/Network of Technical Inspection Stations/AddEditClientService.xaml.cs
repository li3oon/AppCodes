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
    /// Логика взаимодействия для AddEditClientService.xaml
    /// </summary>
    public partial class AddEditClientService : Page
    {
        private Order CurrentOrder = new Order(); // Текущий заказ
        public AddEditClientService(Order selectesOrder)
        {
            InitializeComponent();

            if (selectesOrder != null )
                CurrentOrder = selectesOrder;
            DataContext = CurrentOrder;

            ComboClientID.ItemsSource = DataBase.GetContext().Client.ToList();
            ComboNameService.ItemsSource = DataBase.GetContext().Service.ToList();
            ComboStatus.ItemsSource = DataBase.GetContext().Status.ToList();
        }

        private void BtnSaveClientService_Click(object sender, RoutedEventArgs e)
        {
            DateTime? createdDate = Date1.SelectedDate;
            DateTime? completedDate = Date2.SelectedDate;

            StringBuilder errors = new StringBuilder();

            if (ComboClientID.SelectedItem == null)
                errors.AppendLine("Выберите клиента.");
            if (ComboNameService.SelectedItem == null)
                errors.AppendLine("Выберите услугу.");
            if (ComboStatus.SelectedItem == null)
                errors.AppendLine("Выберите статус.");

            if (createdDate == null)
                errors.AppendLine("Укажите дату создания.");
            else
                CurrentOrder.CreatedDate = createdDate.Value;

            if (completedDate.Value < createdDate.Value)
                errors.AppendLine("Дата завершения не может быть раньше даты создания.");
            else
                CurrentOrder.CompletedDate = completedDate;

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allOrders = DataBase.GetContext().Order.ToList();
            allOrders = allOrders.Where(o => o.ID == CurrentOrder.ID).ToList();
            if (allOrders.Count == 0 || (CurrentOrder.ID != 0 && allOrders.Count <= 1))
            {
                if (allOrders.Count == 0)
                    DataBase.GetContext().Order.Add(CurrentOrder);
                try
                {
                    DataBase.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена.");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Такая услуга уже существует.");
            }
        }
    }
}
