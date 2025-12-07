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

namespace DA_001
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : Page
    {
        Database1Entities1 db = new Database1Entities1();
        public test()
        {
            InitializeComponent();
            load1();
        }
        private void load1()
        {
            tblbang.ItemsSource = db.DonHangs.ToList();
           
        }
       
        public DateTime getdate()
        {
            if (ngay.SelectedDate == null) return DateTime.Now;
            else return ngay.SelectedDate.Value;

        }


        private void click_them(object sender, RoutedEventArgs e)
        {
            DonHang dh = new DonHang
            {
                madon = txt1.Text,
                makh = txt2.Text,
                tenkh = txt3.Text,
                sdt = int.Parse(txt4.Text),
                diachi = txt5.Text,
                ngaydat = getdate()
            };
            db.DonHangs.Add(dh);
            db.SaveChanges();
            load1();
        }

       
        private void Xem_click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button.DataContext as DonHang;

            if (row == null)
                return;

            string maDon = row.madon;
            grid10.Visibility = Visibility.Collapsed;
            Mainfram1.Visibility = Visibility.Visible;
            Mainfram1.Navigate(new test2(maDon));
            
        }
        private void sua_click(object sender, RoutedEventArgs e)
        {
            DonHang dhdc = tblbang.SelectedItem as DonHang;
            DonHang dh = db.DonHangs.Find(dhdc.madon);
            dh.madon = txt1.Text;
            dh.makh = txt2.Text;
            dh.tenkh = txt3.Text;
            dh.sdt = int.Parse(txt4.Text);
            dh.diachi = txt5.Text;
            dh.ngaydat = getdate();
            db.SaveChanges();
            load1();

        }

        private void Tblbang_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            DonHang dhdc = tblbang.SelectedItem as DonHang;
            if (dhdc != null)
            {
                txt1.Text = dhdc.madon;
                txt2.Text = dhdc.makh;
                txt3.Text = dhdc.tenkh;
                txt4.Text = dhdc.sdt.ToString();
                txt5.Text = dhdc.diachi;
                ngay.SelectedDate = dhdc.ngaydat;
            }
        }
        private void xoa_click(object sender, RoutedEventArgs e)
        {
            DonHang dhdc = tblbang.SelectedItem as DonHang;
            DonHang dh = db.DonHangs.Find(dhdc.madon);
            db.DonHangs.Remove(dh);
            db.SaveChanges();
            load1();

        }
    }
}
