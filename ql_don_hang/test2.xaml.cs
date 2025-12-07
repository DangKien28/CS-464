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
    /// Interaction logic for test2.xaml
    /// </summary>
    public partial class test2 : Page
    {
        
        Database1Entities1 db = new Database1Entities1();
        private string ma;
        public test2(string maDon)
        {
            InitializeComponent();
            ma = maDon;
            load2();
            
        }
        public void load2()
        {

            var data = db.CTDonHangs
                         .Where(x => x.madon == ma)
                         .ToList();

            tblbang.ItemsSource = data;



        }
 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CTDonHang ct = new CTDonHang {
                madon=ma,
                masp=txt7.Text,
                tensp=txt8.Text,
                soluong=int.Parse(txt9.Text),
                gia=decimal.Parse(txt10.Text)

            };
            db.CTDonHangs.Add(ct);
            db.SaveChanges();
            load2();

        }

       


       
        private void sua_click(object sender, RoutedEventArgs e)
        {
            CTDonHang dhdc = tblbang.SelectedItem as CTDonHang;
            CTDonHang dh = db.CTDonHangs.Find(dhdc.madon);
            dh.madon = txt7.Text;
            dh.masp = txt8.Text;
          
            dh.soluong = int.Parse(txt9.Text);
            dh.gia =decimal.Parse(txt10.Text);

            db.SaveChanges();
            load2();

        }

        private void Tblbang_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            CTDonHang dhdc = tblbang.SelectedItem as CTDonHang;
            if (dhdc != null)
            {
                txt7.Text = dhdc.madon;
                txt8.Text = dhdc.masp;
               
                txt10.Text = dhdc.soluong.ToString();
                txt10.Text = dhdc.gia.ToString();

            }
        }
        private void xoa_click(object sender, RoutedEventArgs e)
        {
            CTDonHang dhdc = tblbang.SelectedItem as CTDonHang;
            CTDonHang dh = db.CTDonHangs.Find(dhdc.madon,dhdc.masp);
            db.CTDonHangs.Remove(dh);
            db.SaveChanges();
            load2();

        }
    }
}
