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

namespace Libraly.Pages
{
    /// <summary>
    /// Логика взаимодействия для LibrarianView.xaml
    /// </summary>
    public partial class LibrarianView : Page
    {
        private User _currentUser;
        public LibrarianView(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void BtnBooks_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new BookGenre(_currentUser));
        }

        private void BtnReaders_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ReaderList());
        }

        private void BtnReaderBook_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ReaderBookList());
        }
    }
}
