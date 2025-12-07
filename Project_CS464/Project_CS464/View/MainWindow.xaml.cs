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
using Project_CS464;

namespace Project_CS464.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Overview());
        }
        public void openSanPhamPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page1());
        }

        public void openDonHangPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DonHang1());
        }
        public void openBaoCaoPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BaoCaoPage());
        }

        public void openQLNVPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new QLyNVPage());
        }
        public void openAIPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AIPage());
        }
    }
}
