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

namespace Project_CS464
{
  
    public partial class DonHang1 : Page
    {
        QLBHEntities db = new QLBHEntities();
        public DonHang1()
        {
            InitializeComponent();
            load1();
        }
        private void load1()
        {
            //tblbang.ItemsSource = db.DonHangs.ToList();
            var data = db.DonHangs.ToList();

            foreach (var d in data)
            {
                d.TongTien = d.CTDonHangs
                                .Select(ct => (decimal?)(ct.soluong ?? 0) *
                                               (decimal?)(ct.SanPham.Gia ?? 0))
                                .Sum() ?? 0;
            }

            tblbang.ItemsSource = data;




        }
        public DateTime getdate()
        {
            if (ngay.SelectedDate == null) return DateTime.Now;
            else return ngay.SelectedDate.Value;

        }
        public string gettrangthai()
        {
            if (box1.SelectedItem != null)
            {
                ComboBoxItem items = box1.SelectedItem as ComboBoxItem;
                return items.Content.ToString();
            }
            return "";
        }

        private void Them_click(object sender, RoutedEventArgs e)
        {
            DonHang dh = new DonHang
            {
                madon = txt1.Text,
                makh = txt2.Text,
                tenkh = txt3.Text,
                sdt = txt4.Text,
                diachi = txt5.Text,
                ngaydat = getdate(),
                manv = txt6.Text,
                trangthai = gettrangthai()
                
            };
            db.DonHangs.Add(dh);
            db.SaveChanges();
            load1();
        }
        private void xem_click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button.DataContext as DonHang;
            if (row == null)
                return;
            string maDon = row.madon;
            grid10.Visibility = Visibility.Collapsed;
            Mainfram1.Visibility = Visibility.Visible;
            Mainfram1.Navigate(new CTDonHang1(maDon));

        }

        private void tim_click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            // 1. Lọc DonHang theo từ khóa
            var result = db.DonHangs
                .Where(s =>
                    s.madon.Contains(keyword) ||
                    s.tenkh.Contains(keyword) ||
                    s.makh.Contains(keyword) ||
                    s.manv.Contains(keyword)
                )
                .ToList();
            foreach (var d in result)
            {
                d.TongTien = d.CTDonHangs
                                .Select(ct => (decimal?)(ct.soluong ?? 0) *
                                               (decimal?)(ct.SanPham.Gia ?? 0))
                                .Sum() ?? 0;
            }
            tblbang.ItemsSource = result;
        }



        private void Sua_click(object sender, RoutedEventArgs e)
        {
            DonHang dhdc = tblbang.SelectedItem as DonHang;
            DonHang dh = db.DonHangs.Find(dhdc.madon);
            dh.madon = txt1.Text;
            dh.makh = txt2.Text;
            dh.tenkh = txt3.Text;
            dh.sdt = txt4.Text;
            dh.diachi = txt5.Text;
            dh.ngaydat = getdate();
            dh.manv = txt6.Text;
            dh.trangthai = gettrangthai();
            db.SaveChanges();
            load1();
        }
        private void xoa_click(object sender, RoutedEventArgs e)
        {
            DonHang dhdc = tblbang.SelectedItem as DonHang;
            DonHang dh = db.DonHangs.Find(dhdc.madon);
            db.DonHangs.Remove(dh);
            db.SaveChanges();
            load1();


        }

        private void Tblbang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DonHang dhdc = tblbang.SelectedItem as DonHang;
            if (dhdc != null)
            {
                txt1.Text = dhdc.madon;
                txt2.Text = dhdc.makh;
                txt3.Text = dhdc.tenkh;
                txt4.Text = dhdc.sdt;
                txt5.Text = dhdc.diachi;
                ngay.SelectedDate = dhdc.ngaydat;
                box1.SelectedItem = dhdc.trangthai;
                txt6.Text = dhdc.manv;
            }
        }
    }
}
