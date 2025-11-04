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
    /// Логика взаимодействия для ReaderBookList.xaml
    /// </summary>
    public partial class ReaderBookList : Page
    {
        public ReaderBookList()
        {
            InitializeComponent();
            ReaderBookGrid.ItemsSource = DataBase2.GetContext().ReaderBook.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Pages.AddEditReaderBook((sender as Button).DataContext as Libraly.ReaderBook));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Pages.AddEditReaderBook(null));
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var ReaderBooksForRemoving = ReaderBookGrid.SelectedItems.Cast<Libraly.ReaderBook>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {ReaderBooksForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DataBase2.GetContext().ReaderBook.RemoveRange(ReaderBooksForRemoving);
                    DataBase2.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    ReaderBookGrid.ItemsSource = DataBase2.GetContext().ReaderBook.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                ReaderBookGrid.ItemsSource = DataBase2.GetContext().ReaderBook.ToList();

                var entries = DataBase2.GetContext()
                    .ChangeTracker
                    .Entries()
                    .Where(p => p.State != EntityState.Added && p.State != EntityState.Detached)
                    .ToList();

                entries.ForEach(p =>
                {
                    try { p.Reload(); }
                    catch { /* логировать при необходимости */ }
                });
            }
        }
    }
}
