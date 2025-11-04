using Libraly.Pages;
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
using System.Windows.Threading;

namespace Libraly
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

            var User = DataBase2.GetContext().User.FirstOrDefault(u => u.Login == Login && u.Password == Password);
            if (User != null)
            {
                if (User.Role == "Библиотекарь")
                {
                    MessageBox.Show("Вы зашли как Библиотекарь");
                    Manager.MainFrame.Navigate(new LibrarianView(User));
                }
                else if (User.Role == "Читатель")
                {
                    MessageBox.Show("Вы зашли как Читатель");
                    Manager.MainFrame.Navigate(new Pages.BookGenre(User));
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
