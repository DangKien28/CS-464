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
using System.Data.Entity;
using Project_CS464;


namespace Project_CS464
{
    /// <summary>
    /// Interaction logic for CTDonHang.xaml
    /// </summary>
    public partial class CTDonHang1 : Page
    {
        QLBHEntities db = new QLBHEntities();
        private string ma;
        public CTDonHang1(string maDon)
        {
            InitializeComponent();
            ma = maDon;
            load();
        }
        
        public void load()
        {
            var data = db.CTDonHangs
                         .Where(x => x.madon == ma).Include("SanPham")
                         .ToList();

            tblbang.ItemsSource = data;
        }
        
        private void them_click(object sender, RoutedEventArgs e)
        {
            CTDonHang dh = new CTDonHang
            {
                madon = ma,
                masp = txt7.Text,
                
                soluong = int.Parse(txt9.Text),
                
            };
            dh.SanPham = db.SanPhams.Find(dh.masp);
            db.CTDonHangs.Add(dh);
            db.SaveChanges();
          
            load();
        }
        private void xoa_click(object sender, RoutedEventArgs e)
        {
            CTDonHang dhdc = tblbang.SelectedItem as CTDonHang;
            CTDonHang dh = db.CTDonHangs.Find(dhdc.madon, dhdc.masp);
            db.CTDonHangs.Remove(dh);
            db.SaveChanges();
            load();   
        }
        private void Sua_click(object sender, RoutedEventArgs e)
        {
            CTDonHang dhdc = tblbang.SelectedItem as CTDonHang;
            CTDonHang dh = db.CTDonHangs.Find(dhdc.madon, dhdc.masp);

            dh.masp = txt7.Text;
            dh.soluong = int.Parse(txt9.Text);
            db.SaveChanges();
            load();

        }

        private void Tblbang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            CTDonHang dhdc = tblbang.SelectedItem as CTDonHang;
            if (dhdc != null)
            {
                txt7.Text = dhdc.masp;
                txt9.Text = dhdc.soluong.ToString();
                
            }
        }
    }
}
