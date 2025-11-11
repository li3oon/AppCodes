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

namespace Flights
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

            var User = DataBase.GetContext().User.FirstOrDefault(u => u.Login == Login && u.Password == Password);
            if (User != null)
            {
                Manager.CurrentUser = User; //сохранение пользователя для вывода списка его билетов

                if (User.Role == "Кассир")
                {
                    MessageBox.Show("Вы зашли как Кассир");
                    Manager.MainFrame.Navigate(new MainMenu());
                }
                else if (User.Role == "Покупатель")
                {
                    MessageBox.Show("Вы зашли как Покупатель");
                    Manager.MainFrame.Navigate(new MainMenu());
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
