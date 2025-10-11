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
    /// Логика взаимодействия для AddEditService.xaml
    /// </summary>
    public partial class AddEditService : Page
    {
        private Service CurrentService = new Service(); // Текущая услуга
        public AddEditService(Service selectedService)
        {
            InitializeComponent();

            if (selectedService != null)
                CurrentService = selectedService;

            DataContext = CurrentService;
        }

        private void BtnSaveService_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(CurrentService.NameService))
                errors.AppendLine("Укажите название услуги.");
            if (string.IsNullOrWhiteSpace(CurrentService.DescriptionService))
                errors.AppendLine("Укажите описание услуги.");
            if (CurrentService.Cost <= 0)
                errors.AppendLine("Стоимость услуги должна быть больше нуля.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = DataBase.GetContext().Service.ToList();
            allServices = allServices.Where(s => s.ID == CurrentService.ID).ToList();

            if (allServices.Count == 0 || (CurrentService.ID != 0 && allServices.Count <= 1)) { 
                if (allServices.Count == 0)
                    DataBase.GetContext().Service.Add(CurrentService);
                try
                {
                    DataBase.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена.");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex) { 
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else MessageBox.Show("Данная услуга уже существует.");
        }
    }
}
