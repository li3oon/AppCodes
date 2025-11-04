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
    /// Логика взаимодействия для AddEditReaderBook.xaml
    /// </summary>
    public partial class AddEditReaderBook : Page
    {
        private Libraly.ReaderBook _currentReaderBook = new Libraly.ReaderBook();

        public AddEditReaderBook(ReaderBook selectedReaderBook)
        {
            InitializeComponent();

            ComboReaderID.ItemsSource = DataBase2.GetContext().Reader.ToList();
            ComboBook.ItemsSource = DataBase2.GetContext().Book.ToList();

            if (selectedReaderBook != null)
                _currentReaderBook = selectedReaderBook;

            DataContext = _currentReaderBook;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (ComboReaderID.SelectedItem == null)
                stringBuilder.AppendLine("Выберите читателя");
            if (ComboBook.SelectedItem == null)
                stringBuilder.AppendLine("Выберите книгу");
            if (stringBuilder.Length > 0)
            {
                MessageBox.Show(stringBuilder.ToString());
                return;
            }

            // Гарантируем, что ключевые поля модели заполнены (на случай если биндинг не обновил модель)
            _currentReaderBook.ReaderID = (int)ComboReaderID.SelectedValue;
            _currentReaderBook.BookID = ComboBook.SelectedValue as string;

            try
            {
                if (_currentReaderBook.ID == 0)
                    DataBase2.GetContext().ReaderBook.Add(_currentReaderBook);

                DataBase2.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены");
                if (Manager.MainFrame.CanGoBack)
                    Manager.MainFrame.GoBack();
                else
                    Manager.MainFrame.Navigate(new ReaderBookList());
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
            {
                var inner = dbEx.InnerException?.InnerException?.Message ?? dbEx.InnerException?.Message ?? dbEx.Message;
                MessageBox.Show("Ошибка при сохранении: " + inner);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
    }
}
