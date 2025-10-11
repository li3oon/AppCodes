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
            string login = UserLogin.Text;
            string password = UserPassword.Password;

            var user = DataBase.GetContext().User.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                if (user.Role == "Техник")
                {
                    MessageBox.Show("Вы вошли как техник");
                    Manager.MainFrame.Navigate(new TechnicianView(user));
                }
                else if (user.Role == "Клиент")
                {
                    MessageBox.Show("Вы вошли как клиент");
                    Manager.MainFrame.Navigate(new ClientView(user));
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
