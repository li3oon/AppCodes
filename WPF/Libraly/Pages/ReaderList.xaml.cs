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
    /// Логика взаимодействия для ReaderList.xaml
    /// </summary>
    public partial class ReaderList : Page
    {
        public ReaderList()
        {
            InitializeComponent();
            ReaderGrid.ItemsSource = DataBase2.GetContext().Reader.ToList();
        }
    }
}
