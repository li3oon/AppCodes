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

namespace Libraly.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookGenre.xaml
    /// </summary>
    public partial class BookGenre : Page
    {
        private List<Libraly.BookGenre> _allBookGenres;
        private User _currentUser;
        public BookGenre(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            BookGenreGrid.ItemsSource = DataBase2.GetContext().BookGenre.ToList();

            // Проверка роли и отображение/скрытие элементов управления
            ConfigureUIBasedOnRole();
            LoadData();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (_currentUser == null)
                return;

            if (_currentUser.Role == "Библиотекарь")
            {
                // Для библиотекаря: показываем кнопки для добавления, редактирования и удаления услуг
                BtnAdd.Visibility = Visibility.Visible;
                BtnDel.Visibility = Visibility.Visible;

            }
            else if (_currentUser.Role == "Читатель")
            {
                // Для читателя: скрываем кнопки редактирования и удаления, только просмотр
                BtnAdd.Visibility = Visibility.Collapsed;
                BtnDel.Visibility = Visibility.Collapsed;

                // Скрываем кнопку редактирования
                foreach (var column in BookGenreGrid.Columns)
                {
                    if (column is DataGridTemplateColumn templateColumn)
                    {
                        if (templateColumn.Header == null)
                        {
                            templateColumn.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void LoadData()
        {
            _allBookGenres = DataBase2.GetContext().BookGenre
                .Include(bg => bg.Book)
                .Include(bg => bg.Genre)
                .ToList();

            BookGenreGrid.ItemsSource = _allBookGenres;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditBookGenre(_currentUser, (sender as Button).DataContext as Libraly.BookGenre));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditBookGenre(_currentUser));
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var BookGenresForRemoving = BookGenreGrid.SelectedItems.Cast<Libraly.BookGenre>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {BookGenresForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DataBase2.GetContext().BookGenre.RemoveRange(BookGenresForRemoving);
                    DataBase2.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    BookGenreGrid.ItemsSource = DataBase2.GetContext().BookGenre.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void txtFilterCatalog1(object sender, TextChangedEventArgs e)
        {
            string filterText = txtfilter1.Text.ToLower();

            if (string.IsNullOrWhiteSpace(filterText))
            {
                BookGenreGrid.ItemsSource = _allBookGenres;
            }
            else
            {
                BookGenreGrid.ItemsSource = _allBookGenres
                    .Where(bg => bg.Genre != null &&
                                 bg.Genre.NameGenre.ToLower().Contains(filterText))
                    .ToList();
            }
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                BookGenreGrid.ItemsSource = DataBase2.GetContext().BookGenre.ToList();
                DataBase2.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ConfigureUIBasedOnRole();
            }
        }
    }
}
