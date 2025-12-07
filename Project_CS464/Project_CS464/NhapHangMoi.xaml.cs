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
using System.Windows.Shapes;
using Project_CS464.Model;
using Project_CS464;

namespace Project_CS464
{
    /// <summary>
    /// Interaction logic for NhapHangMoi.xaml
    /// </summary>
    public partial class NhapHangMoi : Window
    {
        private SanPhamEntities db = new SanPhamEntities();
        public NhapHangMoi()
        {

            InitializeComponent();
            cboHang.SelectedIndex = 0;
            dpNgayNhap.SelectedDate = DateTime.Now;
        }

        // ===================== LƯU ======================
        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string maSP = txtMaSP.Text.Trim();
                string hang = (cboHang.SelectedItem as ComboBoxItem)?.Content.ToString();
                string mauGiay = txtMauGiay.Text.Trim();
                int size = int.Parse(txtSize.Text.Trim());
                int soLuong = int.Parse(txtSoLuong.Text.Trim());
                int gia = int.Parse(txtGia.Text.Trim());
                DateTime ngayNhap = dpNgayNhap.SelectedDate ?? DateTime.Now;

                if (string.IsNullOrEmpty(maSP) || string.IsNullOrEmpty(hang) || string.IsNullOrEmpty(mauGiay))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SanPham sp = new SanPham()
                {
                    MaSP = maSP,
                    Hang = hang,
                    MauGiay = mauGiay,
                    Size = size,
                    SoLuong = soLuong,
                    Gia = gia,
                    NgayNhap = ngayNhap
                };

                db.SanPhams.Add(sp);
                db.SaveChanges();

                // ---- Trả kết quả cho Page1 ----
                this.DialogResult = true; // thông báo đã lưu thành công
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // người dùng hủy
            this.Close();
        }
    }
}