using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для AddEditBookGenre.xaml
    /// </summary>
    public partial class AddEditBookGenre : Page
    {
        private Libraly.BookGenre _currentBookGenre;

        private User _currentUser;

        public AddEditBookGenre(User currentUser, Libraly.BookGenre selectedBookGenre = null)
        {
            InitializeComponent();

            _currentUser = currentUser;
            ComboNameGenre.ItemsSource = DataBase2.GetContext().Genre.ToList();

            if (selectedBookGenre != null)
            {
                _currentBookGenre = selectedBookGenre;

                // Получаем объект Book по BookID (в сущности BookGenre BookID хранит ISBN)
                var book = DataBase2.GetContext().Book.FirstOrDefault(b => b.ISBN == _currentBookGenre.BookID);
                if (book != null)
                {
                    ISBN.Text = book.ISBN;
                    NameBook.Text = book.NameBook;
                    AuthorLastName.Text = book.AuthorLastName;
                    AuthorFirstName.Text = book.AuthorFirstName;
                    AuthorPatronymic.Text = book.AuthorPatronymic;
                    YearOfPublication.SelectedDate = book.YearOfPublication;
                    Publisher.Text = book.Publisher;
                }
                ComboNameGenre.SelectedValue = _currentBookGenre.GenreID;
            }
            else
            {
                // Создаём новый экземпляр сущности BookGenre
                _currentBookGenre = new Libraly.BookGenre();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            DateTime? Date1 = YearOfPublication.SelectedDate;
            
            if (string.IsNullOrWhiteSpace(ISBN.Text))
                errors.AppendLine("Укажите ISBN книги.");
            if (string.IsNullOrWhiteSpace(NameBook.Text))
                errors.AppendLine("Укажите название книги.");
            if (string.IsNullOrWhiteSpace(AuthorLastName.Text))
                errors.AppendLine("Укажите фамилию автора.");
            if (string.IsNullOrWhiteSpace(AuthorFirstName.Text))
                errors.AppendLine("Укажите имя автора.");
            if (string.IsNullOrWhiteSpace(Publisher.Text))
                errors.AppendLine("Укажите издательство.");
            if (Date1 == null)
                errors.AppendLine("Укажите год издания.");
            if (ComboNameGenre.SelectedItem == null)
                errors.AppendLine("Укажите жанр книги.");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var context = DataBase2.GetContext();

            // Создаём или обновляем запись Book по ISBN
            var book = context.Book.FirstOrDefault(b => b.ISBN == ISBN.Text);
            if (book == null)
            {
                book = new Book
                {
                    ISBN = ISBN.Text,
                    NameBook = NameBook.Text,
                    AuthorLastName = AuthorLastName.Text,
                    AuthorFirstName = AuthorFirstName.Text,
                    AuthorPatronymic = AuthorPatronymic.Text,
                    YearOfPublication = Date1.Value,
                    Publisher = Publisher.Text
                };
                context.Book.Add(book);
            }
            else
            {
                // Обновляем существующую книгу
                book.NameBook = NameBook.Text;
                book.AuthorLastName = AuthorLastName.Text;
                book.AuthorFirstName = AuthorFirstName.Text;
                book.AuthorPatronymic = AuthorPatronymic.Text;
                book.YearOfPublication = Date1.Value;
                book.Publisher = Publisher.Text;
            }

            // Подготавливаем BookGenre (связка)
            int selectedGenreId = (int)ComboNameGenre.SelectedValue;
            _currentBookGenre.BookID = book.ISBN;
            _currentBookGenre.GenreID = selectedGenreId;

            // Проверка на дубликат: существует ли уже запись с такой же BookID и GenreID
            var duplicates = context.BookGenre
                .Where(bg => bg.BookID == _currentBookGenre.BookID && bg.GenreID == _currentBookGenre.GenreID)
                .ToList();

            bool canSave = false;
            if (_currentBookGenre.ID == 0)
            {
                // добавление: запрещаем, если уже есть такая связка
                canSave = duplicates.Count == 0;
            }
            else
            {
                // редактирование: разрешено, если либо нет дубликатов, либо единственный дубликат — это текущая запись
                if (duplicates.Count == 0)
                    canSave = true;
                else if (duplicates.Count == 1 && duplicates[0].ID == _currentBookGenre.ID)
                    canSave = true;
                else
                    canSave = false;
            }

            if (!canSave)
            {
                MessageBox.Show("Такая связка книга-жанр уже существует.");
                return;
            }

            // Добавление BookGenre при необходимости
            if (_currentBookGenre.ID == 0)
            {
                context.BookGenre.Add(_currentBookGenre);
            }

            try
            {
                context.SaveChanges();
                MessageBox.Show("Данные сохранены");
                // возврат к списку
                if (Manager.MainFrame.CanGoBack)
                    Manager.MainFrame.GoBack();
                else
                    Manager.MainFrame.Navigate(new BookGenre(_currentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
    }
}
