using Bus_route_automation_system.Pages;
using Bus_route_automation_system.Pages.Dispetcher;
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

namespace Bus_route_automation_system
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string Login = UserLogin.Text;
            string Password = UserPassword.Password;

            var User = DataBase3.GetContext().User.FirstOrDefault(u=>u.Login == Login && u.Password == Password);
            if (User != null) {
                if (User.Role == "Диспетчер") {
                    MessageBox.Show("Вы зашли как Диспетчер");
                    Manager.MainFrame.Navigate(new Dispetcher(User));
                }
                else if (User.Role == "Водитель") {
                    MessageBox.Show("Вы зашли как Водитель");
                    Manager.MainFrame.Navigate(new DriverVoyages(User));
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
